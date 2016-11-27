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
            $http({
                method: 'GET',
                url: "http://localhost:8641/api/network/GetListOfNetworks"
            })
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

    }
]);
myApp.controller("myCtrl",
[
    "$location", "$http", "$scope", "myService", function ($location, $http, $scope, myService) {
        $scope.vert = myService;

        //to niżej - do znajdywania kolejnego indeksu (nie wykorzystywane teraz)
        $scope.nextindex = function () {
            $http.get("http://localhost:8641/api/network/GetNetwork/" + $scope.selectedNetwork.id + "?date=" + $scope.selectedDate + "&incl=" + $scope.checkInclude)
                .success(function (data) {
                    var max = Math.max.apply(Math, data.vertices.map(function (o) { return o.id; }));
                    myService.toggle(max + 1, null);
                })
                .error(function (data) { alert(data) });
        };

        $scope.$watch("ref",
            function () {
                $http.get("http://localhost:8641/api/network/GetFactors/" + $scope.selectedNetwork.id + "?date=" + $scope.selectedDate)
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
        $scope.disabled = false;
        $scope.showFactors = false;
        $scope.disableButton = function() {
            $scope.disabled = true;
        };
        $scope.enableButton = function() {
            $scope.disabled = false;
        }
        $scope.countFactors = function () {
            if ($scope.checkIndegree && $scope.checkOutdegree && $scope.checkBetweenness && $scope.checkCloseness && $scope.checkInfluence && $scope.checkDensity)
                $http.get("http://localhost:8641/api/CountFactors/CountAll/" +
                        $scope.selectedNetwork.id +
                        "?date=" +
                        $scope.selectedDate)
                    .success(function () {
                        $scope.disabled = false;
                        $scope.refresh();
                    });
            else if ($scope.checkIndegree)
                $http.get("http://localhost:8641/api/CountFactors/IndegreeCentrality/" +
                        $scope.selectedNetwork.id +
                        "?date=" +
                        $scope.selectedDate)
                    .success(function () {
                        $scope.disabled = false;
                        $scope.refresh();
                    });
            else if ($scope.checkOutdegree)
                $http.get("http://localhost:8641/api/CountFactors/OutdegreeCentrality/" +
                        $scope.selectedNetwork.id +
                        "?date=" +
                        $scope.selectedDate)
                    .success(function () {
                        $scope.disabled = false;
                        $scope.refresh();
                    });
            else if ($scope.checkBetweenness)
                $http.get("http://localhost:8641/api/CountFactors/BetweennessCentrality/" +
                        $scope.selectedNetwork.id +
                        "?date=" +
                        $scope.selectedDate)
                    .success(function () {
                        $scope.disabled = false;
                        $scope.refresh();
                    });
            else if ($scope.checkCloseness)
                $http.get("http://localhost:8641/api/CountFactors/ClosenessCentrality/" +
                        $scope.selectedNetwork.id +
                        "?date=" +
                        $scope.selectedDate)
                    .success(function () {
                        $scope.disabled = false;
                        $scope.refresh();
                    });
            else if ($scope.checkInfluence)
                $http.get("http://localhost:8641/api/CountFactors/InfluenceRange/" +
                        $scope.selectedNetwork.id +
                        "?date=" +
                        $scope.selectedDate)
                    .success(function () {
                        $scope.disabled = false;
                        $scope.refresh();
                    });
            else if ($scope.checkDensity)
                $http.get("http://localhost:8641/api/CountFactors/Density/" +
                        $scope.selectedNetwork.id +
                        "?date=" +
                        $scope.selectedDate)
                    .success(function () {
                        $scope.disabled = false;
                        $scope.refresh();
                    });
            if (!$scope.checkIndegree && !$scope.checkOutdegree && !$scope.checkBetweenness && !$scope.checkCloseness && !$scope.checkInfluence && !$scope.checkDensity) {
                alert("nic nie zaznaczono");
            }

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

        function toggle(indexvalue, edgesvalue, namevalue) {
            service.index = indexvalue;
            service.edges = edgesvalue;
            service.name = namevalue;
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
                            .force("link", d3.forceLink().distance(150).strength(0.45))
                            .force("charge", d3.forceManyBody().strength(-150).distanceMax(200))
                            .force("center", d3.forceCenter(fw / 2, fh / 2))
                            .force("collide", d3.forceCollide(10).strength(0.9));


                        d3.json("http://localhost:8641/api/network/GetNetwork/" + $scope.selectedNetwork.id + "?date=" + $scope.selectedDate + "&vertid=" + $scope.selectedVertex.id + "&incl=" + +$scope.checkInclude,
                            function (error, graph) {
                                if (error) alert(error);


                                var nodes = graph.vertices,
                                    nodeById = d3.map(nodes, function (d) { return d.id; }),
                                    links = graph.links,
                                    bilinks = [];


                                var colorScale = d3.scaleLinear()
                                    .domain([
                                        0, d3.max(nodes, function (d) { return d.closenessCentralityValue; }) / 2, d3
                                        .max(nodes, function (d) { return d.closenessCentralityValue; })
                                    ])
                                    .range(["#e70000", "#f6da2f", "#73f224"])
                                    .interpolate(d3.interpolateHcl);

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
                                            var num = d.closenessCentralityValue;
                                            return colorScale(num);
                                            
                                        }
                                    )
                                    .on("mouseover",
                                        function () {
                                            d3.select(this).attr("fill", "#494bd2");
                                        })
                                    .on("mouseout",
                                        function () {
                                            d3.select(this)
                                                .attr("fill",
                                                    function (d) {
                                                        var num = d.closenessCentralityValue;
                                                        return colorScale(num);

                                                    });
                                        })
                                    .on("click",
                                        function (d) {
                                            $timeout(myService.toggle(d.identifier, d.edges.length, d.name));
                                        })
                                    .call(d3.drag()
                                        .on("start", dragstarted)
                                        .on("drag", dragged)
                                        .on("end", dragended));

                                // strzałki
                                svg.append("svg:defs")
                                    .selectAll("marker")
                                    .data(["end"]) 
                                    .enter()
                                    .append("svg:marker") 
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
                                            "\nbetweenness centrality: " +
                                            d.betweennessCentralityValue +
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
                            if (!d3.event.active) simulation.alphaTarget(0.02).restart();
                            d.fx = d.x, d.fy = d.y;
                        }

                        function dragged(d) {
                            d.fx = d3.event.x, d.fy = d3.event.y;
                        }

                        function dragended(d) {
                            if (!d3.event.active) simulation.alphaTarget(0);
                            d.fx = null, d.fy = null;
                        }

                       
                    });

            }
        };
    }
])