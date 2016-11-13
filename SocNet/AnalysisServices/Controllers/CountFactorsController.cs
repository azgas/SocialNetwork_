using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using AnalysisServices.Models;

namespace AnalysisServices.Controllers
{
    [EnableCors(origins: "http://localhost:60701", headers: "*", methods: "*")]
    public class CountFactorsController : ApiController
    {

        private Networkv3Entities db = new Networkv3Entities();

        public Network MakeNetworkFromDb(int id, string date, int incl)
        {
            int excluded = new int();
            var searched = db.NetworkDb.ToList().Find(net => net.id == id);
            if (incl == 0)
            {

                List<LinkDb> list = searched.LinkDb.ToList();
                if (list[0].source_id == list[1].source_id)
                {
                    excluded = (int)list[0].source_id;
                }
                else if (list[0].target_id == list[1].target_id)
                {
                    excluded = (int)list[0].target_id;
                }
            }

            DateTime dateT = DateTime.Parse(date);
            Network network_temp = new Network //dane o sieci
            {
                id = (int)searched.id,
                serviceType = searched.service_id,
                name = searched.name
            };
            List<Link> links = new List<Link>(); //tworzenie połączeń
            foreach (var linkdb in db.LinkDb.Where(n => n.network_id == searched.id && n.date_modified == dateT))
                if (incl == 0 && (linkdb.source_id == excluded || linkdb.target_id == excluded))
                    continue;
                else
                {
                    Link link_temp = new Link
                    {
                        source = (int)linkdb.source_id,
                        target = (int)linkdb.target_id
                    };
                    if (!links.Any(o => o.source == link_temp.source && o.target == link_temp.target))
                        links.Add(link_temp);
                    if (!searched.directed)
                    {
                        Link link_temp2 = new Link
                        {
                            source = (int)linkdb.target_id,
                            target = (int)linkdb.source_id
                        };
                        if (!links.Any(o => o.source == link_temp2.source && o.target == link_temp2.target))
                            links.Add(link_temp2);
                    }
                }
            network_temp.links = links;

            List<Vertex> vertices = new List<Vertex>(); //tworzenie wierzchołków
            foreach (var link in network_temp.links)
            {
                if (vertices.All(v => v.id != link.source))
                {
                    var vertex_info = db.VertexDb.ToList().Find(v => v.id == link.source);

                    Vertex vertex_temp = new Vertex
                    {
                        id = (int)vertex_info.id,
                        name = vertex_info.name,
                        identifier = vertex_info.identifier
                    };
                    var vertFactors = db.VertexFactorsDb.ToList().SingleOrDefault(v => (v.vertex_id == vertex_info.id && v.date == dateT && v.up_to_date && v.network_id == searched.id));
                    if (vertFactors != null)
                    {
                        vertex_temp.betweennessCentralityValue = (float)vertFactors.betweenness_centrality;
                        vertex_temp.closenessCentralityValue = (float)vertFactors.closeness_centrality;
                        vertex_temp.indegreeCentralityValue = (float)vertFactors.indegree_centrality;
                        vertex_temp.outdegreeCentralityValue = (float)vertFactors.outdegree_centrality;
                        vertex_temp.influenceRangeValue = (float)vertFactors.influence_range;
                    }

                    List<int> edges = new List<int>();
                    if (vertex_info.LinkDb != null)
                        foreach (var link_containing in vertex_info.LinkDb)
                        {
                            if (link_containing.date_modified == dateT && link_containing.network_id == searched.id)
                            {
                                edges.Add((int)link_containing.target_id);
                                if (!searched.directed)
                                {
                                    edges.Add((int)link_containing.source_id);
                                }
                            }
                        }

                    vertex_temp.edges = edges;
                    vertices.Add(vertex_temp);
                }
                if (vertices.All(v => v.id != link.target))
                {
                    var vertex_info = db.VertexDb.ToList().Find(v => v.id == link.target);

                    Vertex vertex_temp = new Vertex
                    {
                        id = (int)vertex_info.id,
                        name = vertex_info.name,
                        identifier = vertex_info.identifier
                    };
                    var vertFactors = db.VertexFactorsDb.ToList().SingleOrDefault(v => (v.vertex_id == vertex_info.id && v.date == dateT && v.up_to_date));
                    if (vertFactors != null)
                    {
                        vertex_temp.betweennessCentralityValue = (float)vertFactors.betweenness_centrality;
                        vertex_temp.closenessCentralityValue = (float)vertFactors.closeness_centrality;
                        vertex_temp.indegreeCentralityValue = (float)vertFactors.indegree_centrality;
                        vertex_temp.outdegreeCentralityValue = (float)vertFactors.outdegree_centrality;
                        vertex_temp.influenceRangeValue = (float)vertFactors.influence_range;
                    }

                    List<int> edges = new List<int>();
                    if (vertex_info.LinkDb != null)
                        foreach (var link_containing in vertex_info.LinkDb.Where(linkk => linkk.network_id == searched.id))
                        {
                            if (link_containing.date_modified == dateT)
                            {
                                edges.Add((int)link_containing.target_id);
                                if (!searched.directed)
                                {
                                    edges.Add((int)link_containing.source_id);
                                }
                            }
                        }

                    vertex_temp.edges = edges;
                    vertices.Add(vertex_temp);
                }
            }
            network_temp.vertices = vertices;

            var distinctDates = searched.LinkDb.Select(p => p.date_modified).Distinct().ToList();
            network_temp.datesOfUpdates = distinctDates;
            return network_temp;
        }

        [HttpGet]
        public JsonResult<float> BetweennessCentrality(int id, string date)
        {
            Network net = MakeNetworkFromDb(id, date, 1);
            net.BetweennessCentrality2();
            DateTime dateT = DateTime.Parse(date);

            var betweenness = net.AverageFactor(4);
            var previousN = db.NetworkFactorsDb.SingleOrDefault(o => o.network_id == net.id && o.date == dateT && o.up_to_date);
            if (previousN != null)
            {
                var row_ = from o in db.NetworkFactorsDb
                           where o.network_id == net.id && o.date == dateT && o.up_to_date
                           select o;
                foreach (var o in row_)
                {
                    o.avg_betweenness_centrality = betweenness;
                    o.bet_cen_count = true;
                }
                db.SaveChanges();
            }
            else
            {
                var rowN = new NetworkFactorsDb
                {
                    network_id = id,
                    avg_betweenness_centrality = betweenness,
                    date = DateTime.Parse(date),
                    up_to_date = true,
                    bet_cen_count = true
                };
                db.NetworkFactorsDb.Add(rowN);
                db.SaveChanges();
            }

            net.Normalize(); // normalizacja
            
            foreach (Vertex vert in net.vertices) //zapis do tablicy współczynników wierzchołków
            {
                var previous = db.VertexFactorsDb.SingleOrDefault(o => o.vertex_id == vert.id && o.date == dateT && o.up_to_date);
                if (previous != null)
                {
                    var row_ = from o in db.VertexFactorsDb where o.vertex_id == vert.id && o.date == dateT select o;
                    foreach (var o in row_)
                    {
                        o.betweenness_centrality = vert.betweennessCentralityValue;

                    }
                }
                else
                {
                    var row = new VertexFactorsDb
                    {
                        vertex_id = vert.id,
                        betweenness_centrality = vert.betweennessCentralityValue,
                        date = DateTime.Parse(date),
                        up_to_date = true,
                        network_id = id

                    };
                    db.VertexFactorsDb.Add(row);
                    db.SaveChanges();
                }
            }
            
            return Json(betweenness);
        }

        [HttpGet]
        public JsonResult<float> ClosenessCentrality(int id, string date)
        {
            Network net = MakeNetworkFromDb(id, date, 1);
            net.ClosenessCentrality2();
            
            DateTime dateT = DateTime.Parse(date);
            var closeness = net.AverageFactor(3);
            var previousN = db.NetworkFactorsDb.SingleOrDefault(o => o.network_id == net.id && o.date == dateT && o.up_to_date);
            if (previousN != null)
            {
                var row_ = from o in db.NetworkFactorsDb
                           where o.network_id == net.id && o.date == dateT && o.up_to_date
                           select o;
                foreach (var o in row_)
                {
                    o.avg_closeness_centrality = closeness;
                    o.clos_cen_count = true;
                }
                db.SaveChanges();
            }
            else
            {
                var rowN = new NetworkFactorsDb
                {
                    network_id = id,
                    avg_closeness_centrality = closeness,
                    date = DateTime.Parse(date),
                    up_to_date = true,
                    clos_cen_count = true
                };
                db.NetworkFactorsDb.Add(rowN);
                db.SaveChanges();
            }

            net.Normalize();

            foreach (Vertex vert in net.vertices)
            {
                var previous = db.VertexFactorsDb.SingleOrDefault(o => o.vertex_id == vert.id && o.date == dateT && o.up_to_date);
                if (previous != null)
                {
                    var row_ = from o in db.VertexFactorsDb where o.vertex_id == vert.id && o.date == dateT select o;
                    foreach (var o in row_)
                    {
                        o.closeness_centrality = vert.closenessCentralityValue;

                    }
                }
                else
                {
                    var row = new VertexFactorsDb
                    {
                        vertex_id = vert.id,
                        closeness_centrality = vert.closenessCentralityValue,
                        date = DateTime.Parse(date),
                        up_to_date = true,
                        network_id = id

                    };
                    db.VertexFactorsDb.Add(row);
                    db.SaveChanges();
                }
            }
            
            return Json(closeness);
        }

        [HttpGet]
        public JsonResult<float> IndegreeCentrality(int id, string date)
        {
            Network net = MakeNetworkFromDb(id, date, 1);
            DateTime dateT = DateTime.Parse(date);
            net.CentralityIn2();

            var indegree = net.AverageFactor(1);
            var previousN = db.NetworkFactorsDb.SingleOrDefault(o => o.network_id == net.id && o.date == dateT && o.up_to_date);
            if (previousN != null)
            {
                var row_ = from o in db.NetworkFactorsDb
                           where o.network_id == net.id && o.date == dateT && o.up_to_date
                           select o;
                foreach (var o in row_)
                {
                    o.avg_indegree_centrality = indegree;
                    o.ind_cen_count = true;
                }
                db.SaveChanges();
            }
            else
            {
                var rowN = new NetworkFactorsDb
                {
                    network_id = id,
                    avg_indegree_centrality = indegree,
                    date = DateTime.Parse(date),
                    up_to_date = true,
                    ind_cen_count = true
                };
                db.NetworkFactorsDb.Add(rowN);
                db.SaveChanges();
            }
            net.Normalize();
            foreach (Vertex vert in net.vertices)
            {
                var previous = db.VertexFactorsDb.SingleOrDefault(o => o.vertex_id == vert.id && o.date == dateT && o.up_to_date);
                if (previous != null)
                {
                    var row_ = from o in db.VertexFactorsDb where o.vertex_id == vert.id && o.date == dateT select o;
                    foreach (var o in row_)
                    {
                        o.indegree_centrality = vert.indegreeCentralityValue;

                    }
                }
                else
                {
                    var row = new VertexFactorsDb
                    {
                        vertex_id = vert.id,
                        indegree_centrality = vert.indegreeCentralityValue,
                        date = DateTime.Parse(date),
                        up_to_date = true,
                        network_id = id

                    };
                    db.VertexFactorsDb.Add(row);
                    db.SaveChanges();
                }
            }
            
            return Json(indegree);
        }

        [HttpGet]
        public JsonResult<float> InfluenceRange(int id, string date)
        {
            Network net = MakeNetworkFromDb(id, date, 1);
            net.InfluenceRange2();
            DateTime dateT = DateTime.Parse(date);
            var influence = net.AverageFactor(5);
            var previousN = db.NetworkFactorsDb.SingleOrDefault(o => o.network_id == net.id && o.date == dateT && o.up_to_date);
            if (previousN != null)
            {
                var row_ = from o in db.NetworkFactorsDb
                           where o.network_id == net.id && o.date == dateT && o.up_to_date
                           select o;
                foreach (var o in row_)
                {
                    o.avg_influence_range = influence;
                    o.inf_range_count = true;
                }
                db.SaveChanges();
            }
            else
            {
                var rowN = new NetworkFactorsDb
                {
                    network_id = id,
                    avg_influence_range = influence,
                    date = DateTime.Parse(date),
                    up_to_date = true,
                    inf_range_count = true
                };
                db.NetworkFactorsDb.Add(rowN);
                db.SaveChanges();
            }
            net.Normalize();
            
            foreach (Vertex vert in net.vertices)
            {
                var previous = db.VertexFactorsDb.SingleOrDefault(o => o.vertex_id == vert.id && o.date == dateT && o.up_to_date);
                if (previous != null)
                {
                    var row_ = from o in db.VertexFactorsDb where o.vertex_id == vert.id && o.date == dateT select o;
                    foreach (var o in row_)
                    {
                        o.influence_range = vert.influenceRangeValue;

                    }
                }
                else
                {
                    var row = new VertexFactorsDb
                    {
                        vertex_id = vert.id,
                        influence_range = vert.influenceRangeValue,
                        date = DateTime.Parse(date),
                        up_to_date = true,
                        network_id = id

                    };
                    db.VertexFactorsDb.Add(row);
                    db.SaveChanges();
                }
            }
            
            return Json(influence);
        }

        [HttpGet]
        public JsonResult<float> OutdegreeCentrality(int id, string date)
        {
            Network net = MakeNetworkFromDb(id, date, 1);
            net.CentralityOut2();
            DateTime dateT = DateTime.Parse(date);
            var outdegree = net.AverageFactor(2);
            var previousN = db.NetworkFactorsDb.SingleOrDefault(o => o.network_id == net.id && o.date == dateT && o.up_to_date);
            if (previousN != null)
            {
                var row_ = from o in db.NetworkFactorsDb
                           where o.network_id == net.id && o.date == dateT && o.up_to_date
                           select o;
                foreach (var o in row_)
                {
                    o.avg_outdegree_centrality = outdegree;
                    o.out_cen_count = true;
                }
                db.SaveChanges();
            }
            else
            {
                var rowN = new NetworkFactorsDb
                {
                    network_id = id,
                    avg_outdegree_centrality = outdegree,
                    date = DateTime.Parse(date),
                    up_to_date = true,
                    out_cen_count = true
                };
                db.NetworkFactorsDb.Add(rowN);
                db.SaveChanges();
            }
            net.Normalize();

            foreach (Vertex vert in net.vertices)
            {
                var previous = db.VertexFactorsDb.SingleOrDefault(o => o.vertex_id == vert.id && o.date == dateT && o.up_to_date);
                if (previous != null)
                {
                    var row_ = from o in db.VertexFactorsDb where o.vertex_id == vert.id && o.date == dateT select o;
                    foreach (var o in row_)
                    {
                        o.outdegree_centrality = vert.outdegreeCentralityValue;
                    }
                }
                else
                {
                    var row = new VertexFactorsDb
                    {
                        vertex_id = vert.id,
                        outdegree_centrality = vert.outdegreeCentralityValue,
                        date = DateTime.Parse(date),
                        up_to_date = true,
                        network_id = id

                    };
                    db.VertexFactorsDb.Add(row);
                    db.SaveChanges();
                }
            }
            
            return Json(outdegree);
        }

        [HttpGet]
        public JsonResult<double> Density(int id, string date)
        {
            Network net = MakeNetworkFromDb(id, date, 1);
            DateTime dateT = DateTime.Parse(date);

            var density = net.Density();
            var previousN = db.NetworkFactorsDb.SingleOrDefault(o => o.network_id == net.id && o.date == dateT && o.up_to_date);
            if (previousN != null)
            {
                var row_ = from o in db.NetworkFactorsDb
                           where o.network_id == net.id && o.date == dateT && o.up_to_date
                           select o;
                foreach (var o in row_)
                {
                    o.density = density;
                    o.density_count = true;
                }
                db.SaveChanges();
            }
            else
            {
                var rowN = new NetworkFactorsDb
                {
                    network_id = id,
                    density = density,
                    date = DateTime.Parse(date),
                    up_to_date = true,
                    density_count = true
                };
                db.NetworkFactorsDb.Add(rowN);
                db.SaveChanges();
            }
            return Json(density);
        }

        [HttpGet]
        public IHttpActionResult CountAll(int id, string date)
        {
            Network networkTemp = MakeNetworkFromDb(id, date, 1); //liczy zawsze z wierzchołkiem początkowym
            DateTime dateT = DateTime.Parse(date);
            networkTemp.ClosenessCentrality2();
            networkTemp.InfluenceRange2();
            networkTemp.CentralityIn2();
            networkTemp.CentralityOut2();
            networkTemp.BetweennessCentrality2();

            var previousN = db.NetworkFactorsDb.SingleOrDefault(o => o.network_id == networkTemp.id && o.date == dateT && o.up_to_date);
            if (previousN != null)
            {
                var row_ = from o in db.NetworkFactorsDb
                           where o.network_id == networkTemp.id && o.date == dateT && o.up_to_date
                           select o;
                foreach (var o in row_)
                {
                    o.avg_indegree_centrality = networkTemp.AverageFactor(1);
                    o.avg_outdegree_centrality = networkTemp.AverageFactor(2);
                    o.avg_closeness_centrality = networkTemp.AverageFactor(3);
                    o.avg_betweenness_centrality = networkTemp.AverageFactor(4);
                    o.avg_influence_range = networkTemp.AverageFactor(5);
                    o.density = networkTemp.Density();
                    o.bet_cen_count = true;
                    o.clos_cen_count = true;
                    o.density_count = true;
                    o.ind_cen_count = true;
                    o.inf_range_count = true;
                    o.out_cen_count = true;
                }
                db.SaveChanges();
            }
            else
            {
                var rowN = new NetworkFactorsDb
                {
                    network_id = id,
                    avg_indegree_centrality = networkTemp.AverageFactor(1),
                    avg_outdegree_centrality = networkTemp.AverageFactor(2),
                    avg_closeness_centrality = networkTemp.AverageFactor(3),
                    avg_betweenness_centrality = networkTemp.AverageFactor(4),
                    avg_influence_range = networkTemp.AverageFactor(5),
                    density = networkTemp.Density(),
                    date = DateTime.Parse(date),
                    up_to_date = true,
                    bet_cen_count = true,
                    clos_cen_count = true,
                    density_count = true,
                    ind_cen_count = true,
                    inf_range_count = true,
                    out_cen_count = true
                };
                db.NetworkFactorsDb.Add(rowN);
                db.SaveChanges();
            }
            networkTemp.Normalize();
            foreach (Vertex vert in networkTemp.vertices)
            {
                //zmienione - nadpisuje stary wiersz
                var previous = db.VertexFactorsDb.SingleOrDefault(o => o.vertex_id == vert.id && o.date == dateT && o.up_to_date);
                if (previous != null)
                {
                    var row_ = from o in db.VertexFactorsDb where o.vertex_id == vert.id && o.date == dateT select o;
                    foreach (var o in row_)
                    {
                        o.betweenness_centrality = vert.betweennessCentralityValue;
                        o.closeness_centrality = vert.closenessCentralityValue;
                        o.indegree_centrality = vert.indegreeCentralityValue;
                        o.influence_range = vert.influenceRangeValue;
                        o.outdegree_centrality = vert.outdegreeCentralityValue;
                    }
                }
                else
                {
                    var row = new VertexFactorsDb
                    {
                        vertex_id = vert.id,
                        betweenness_centrality = vert.betweennessCentralityValue,
                        closeness_centrality = vert.closenessCentralityValue,
                        indegree_centrality = vert.indegreeCentralityValue,
                        influence_range = vert.influenceRangeValue,
                        outdegree_centrality = vert.outdegreeCentralityValue,
                        date = DateTime.Parse(date),
                        up_to_date = true,
                        network_id = id

                    };
                    db.VertexFactorsDb.Add(row);
                    db.SaveChanges();
                }
            }
            

            return Ok();
        }

        [HttpGet]
        public JsonResult<long> GetFactorsId(int id)
        {
            var searched = db.NetworkDb.ToList().Find(net => net.id == id);
            long factors_id = 0;
            factors_id = searched.NetworkFactorsDb.Max(x => x.id);
            return Json(factors_id);

        }
    }
}
