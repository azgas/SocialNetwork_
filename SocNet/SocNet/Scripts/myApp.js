var myApp = angular.module("myApp", []);

myApp.controller("refreshCtrl",
[
    "$location", "$scope", "$http", "myService", function ($location, $scope, $http, myService) {
        $scope.ref = 0; //zmienna, którą obserwuje graf do odświeżania
        $scope.checkInclude = 1;
        $scope.refresh = function () {
            $scope.ref = myService.toggleval($scope.ref);
        };
        $scope.getNetworks = function () {
            $http.get("api/apinetwork/GetListOfNetworks")
                .success(function (data) {
                    $scope.networks = data;
                    $scope.selectedNetwork = data[0];

                    $scope.getDates();
                    $scope.getVertex();
                    $scope.refresh();
                })
                .error(function () {
                    alert("nie udało się pobrać listy sieci");
                });
        }
        $scope.getNetworks();
        $scope.getDates = function () {
            $scope.dates = $scope.selectedNetwork.datesOfUpdates;
            $scope.selectedDate = $scope.dates[0];
        };
        $scope.getVertex = function () {
            $scope.vertices = $scope.selectedNetwork.vertices;
            var all = { "name": "Pokaż całość", "id": 0 };
            $scope.vertices.unshift(all);
            $scope.selectedVertex = $scope.vertices[1];
        };
        $scope.addNew = function () {
            $scope.visible = true;
            $scope.networkName = "Wprowadź nazwę";
            $scope.untouched = true;
        }
        $scope.hide = function () {
            $scope.visible = false;
        }
        $scope.clear = function () {
            if ($scope.networkName === "Wprowadź nazwę") {
                $scope.networkName = "";
                $scope.untouched = false;
            }
        }
        $scope.addNetwork = function () {
            if ($scope.networkName === "" || $scope.untouched) {
                alert("Wprowadź najpierw nazwę!");
            } else {
                var name = $scope.networkName;
                $http.post("api/apinetwork/CreateNew", '"' + name + '"')
                    .success(function () {
                        $scope.getNetworks();
                        $scope.networkName = "";
                        $scope.selectedNetwork = { id: 3 };
                        $scope.hide();
                    })
                    .error(function () { alert("błąd") });
            }
        }
    }
]);
myApp.controller("myCtrl",
[
    "$location", "$http", "$scope", "myService", function ($location, $http, $scope, myService) {
        $scope.vert = myService;

        //to niżej - do znajdywania kolejnego indeksu (nie wykorzystywane teraz)
        $scope.nextindex = function () {
            $http.get("api/apinetwork/GetNetwork/" + $scope.selectedNetwork.id + "?date=" + $scope.selectedDate + "&incl=" + $scope.checkInclude)
                .success(function (data) {
                    var max = Math.max.apply(Math, data.vertices.map(function (o) { return o.id; }));
                    myService.toggle(max + 1, null);
                })
                .error(function (data) { alert(data) });
        };

        // zapisywanie zmian - nie działa teraz
        $scope.save = function () {
            var index = myService.index;
            var edgesTemp = myService.edges;
            var edges = JSON.parse("[" + edgesTemp + "]");

            var vertex = { id: index, edges: edges };
            $http.post("/api/apinetwork/SaveData/" + $scope.selectedNetwork.id, vertex)
                .success(function () {
                    $scope.refresh();
                    myService.toggle(null, null);
                })
                .error(function (err) {
                    alert(err.Message);
                });
        };
        $scope.$watch("ref",
            function () {
                $http.get("/api/apinetwork/GetFactors/" + $scope.selectedNetwork.id + "?date=" + $scope.selectedDate)
                    .success(function (data) {
                        $scope.factors = data;
                        if ($scope.factors.status !== "Nie obliczono jeszcze współczynników dla tej sieci") {
                            $scope.showFactors = true;
                        } else {
                            $scope.showFactors = false;
                        };
                    })
                    .error(function (err) {
                        alert(err.Message);
                    });
            });

        $scope.showFactors = false;
        $scope.countFactors = function () {
            $http.get("/api/apinetwork/Count/" + $scope.selectedNetwork.id + "?date=" + $scope.selectedDate + "?incl=" + $scope.checkInclude)
                .success(function (data) {
                    $scope.refresh();
                })
                .error(function () { alert("błąd") });

        };
        //usuwanie - nie działa teraz
        $scope.delete = function () {
            var index = myService.index;
            $http.post("/api/apinetwork/Delete/" + $scope.selectedNetwork.id, index)
                .success(function () {
                    $scope.refresh();
                    myService.toggle(null, null);
                })
                .error(function (err) {
                    alert(err.Message);
                });
        };
    }
]);

myApp.service("myService",
    function () {
        var service =
        {
            index: "",
            edges: "",
            toggle: toggle,
            toggleval: toggleval
        };

        return service;

        function toggle(indexvalue, edgesvalue) {
            service.index = indexvalue;
            service.edges = edgesvalue;
        }

        function toggleval(value) {
            var toggle = value ? 0 : 1;
            return toggle;
        }
    });

myApp.directive("myGraph",
[
    "$http", "$compile", "$timeout", "myService", function ($http, $compile, $timeout, myService) {
        return {
            restrict: "A",
            scope: false,
            link: function ($scope) {
                $scope.$watch("ref",
                    function () {
                        d3.selectAll("svg > *").remove();
                        var svg = d3.select("svg");
                        var frame = d3.select("svg")
                            .attr("class", "frame");

                        var fh = frame.style("height").replace("px", "");
                        var fw = frame.style("width").replace("px", "");

                        var simulation = d3.forceSimulation()
                            .force("link", d3.forceLink().distance(70).strength(0.40))
                            .force("charge", d3.forceManyBody().distanceMax(120))
                            .force("center", d3.forceCenter(fw / 2, fh / 2));


                        d3.json("/api/apinetwork/GetNetwork/" + $scope.selectedNetwork.id + "?date=" + $scope.selectedDate + "&vertid=" + $scope.selectedVertex.id + "&incl=" + +$scope.checkInclude,
                            function (error, graph) {
                                if (error) alert(error);


                                var nodes = graph.vertices,
                                    nodeById = d3.map(nodes, function (d) { return d.id; }),
                                    links = graph.links,
                                    bilinks = [];


                                links.forEach(function (link) {
                                    var s = link.source = nodeById.get(link.source),
                                        t = link.target = nodeById.get(link.target),
                                        i = {}; // intermediate node
                                    nodes.push(i);
                                    links.push({ source: s, target: i }, { source: i, target: t });
                                    bilinks.push([s, i, t]);
                                });

                                var link = svg.selectAll(".link")
                                    .data(bilinks)
                                    .enter()
                                    .append("path")
                                    .attr("class", "link")
                                    .attr("marker-end", "url(#end)");

                                var node = svg.selectAll(".node")
                                    .data(nodes.filter(function (d) { return d.id; }))
                                    .enter()
                                    .append("circle")
                                    .attr("class", "node")
                                    .attr("r", 7)
                                    .attr("fill",
                                        function (d) {
/*                                            if (d.id === $scope.selectedVertex.id)
                                                return "SlateBlue";
                                            else
                                                */return "black";
                                        }
                                    )
                                    .on("mouseover",
                                        function () {
                                            d3.select(this).attr("fill", "lawnGreen");
                                        })
                                    .on("mouseout",
                                        function () {
                                            d3.select(this)
                                                .attr("fill",
                                                    function (d) {
/*                                                        if (d.id === $scope.selectedVertex.id)
                                                            return "SlateBlue";
                                                        else
                                                            */return "black";
                                                    });
                                        })
                                    .on("click",
                                        function (d) {
                                            $timeout(myService.toggle(d.identifier, d.name));
                                        })
                                    .call(d3.drag()
                                        .on("start", dragstarted)
                                        .on("drag", dragged)
                                        .on("end", dragended));

                                // build the arrow.
                                svg.append("svg:defs")
                                    .selectAll("marker")
                                    .data(["end"]) // Different link/path types can be defined here
                                    .enter()
                                    .append("svg:marker") // This section adds in the arrows
                                    .attr("id", String)
                                    .attr("viewBox", "0 -5 10 10")
                                    .attr("refX", 20)
                                    .attr("refY", -1.5)
                                    .attr("markerWidth", 6)
                                    .attr("markerHeight", 10)
                                    .attr("orient", "auto")
                                    .append("svg:path")
                                    .attr("d", "M0,-5L10,0L0,5");

                                node.append("title")
                                    .text(function (d) {
                                        return "id: " +
                                            d.id +
                                            "\nname: " +
                                            d.name +
                                            "\ncentrality (indegree): " +
                                            d.indegreeCentralityValue +
                                            "\ncentrality (outdegree): " +
                                            d.outdegreeCentralityValue +
                                            "\nbetweeness centrality: " +
                                            d.betweenessCentralityValue +
                                            "\ncloseness centrality: " +
                                            d.closenessCentralityValue +
                                            "\ninfluence range: " +
                                            d.influenceRangeValue;
                                    });

                                simulation
                                    .nodes(nodes)
                                    .on("tick", ticked);

                                simulation.force("link")
                                    .links(links);

                                function ticked() {
                                    link.attr("d", positionLink);
                                    node.attr("transform", positionNode);

                                }

                                /*                    var p = d3.select("svg")
                                                    .append("text")
                                                    .attr("x", 50)
                                                    .attr("y", 50)
                                                    .text(function (d) { return "Gęstość grafu: nie ma bo nie" });*/
                            });

                        function positionLink(d) {
                            return "M" +
                                d[0].x +
                                "," +
                                d[0].y +
                                "S" +
                                d[1].x +
                                "," +
                                d[1].y +
                                " " +
                                d[2].x +
                                "," +
                                d[2].y;
                        }

                        function positionNode(d) {
                            return "translate(" + d.x + "," + d.y + ")";
                        }

                        function dragstarted(d) {
                            if (!d3.event.active) simulation.alphaTarget(0.3).restart();
                            d.fx = d.x, d.fy = d.y;
                        }

                        function dragged(d) {
                            d.fx = d3.event.x, d.fy = d3.event.y;
                        }

                        function dragended(d) {
                            if (!d3.event.active) simulation.alphaTarget(0);
                            d.fx = null, d.fy = null;
                        }

                        //to niżej może posłuszyć do wygaszania, tylko trzeba przerobić
                        /*                function fade(opacity) {
                                            return function (d, i) {
                                                svg.selectAll("circle, .link").style("opacity", opacity);
                                                var associated_links = svg.selectAll(".link").filter(function (d) {
                                                    return d.source.index == i || d.target.index == i;
                                                }).each(function (dLink, iLink) {
                                                    d3.select(this).style("opacity", 1);
                                                    d3.select(dLink.source).style("opacity", 1);
                                                    d3.select(dLink.target).style("opacity", 1);
                                                });
                                            };
                        
                        
                        
                                        };*/
                    });

            }
        };
    }
])