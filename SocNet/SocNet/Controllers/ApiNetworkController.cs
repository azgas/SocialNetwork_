using SocNet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Http.Results;
using System.Web.Hosting;
using AlgorytmyMVC.Models;
using Microsoft.Ajax.Utilities;

namespace AlgorytmyMVC.Controllers
{
    public class ApiNetworkController : ApiController
    {
        private Networkv3Entities1 db = new Networkv3Entities1();
        //OK, POPRAWIONE CHYBA
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
                        vertex_temp.indegreeCentralityValue = vertFactors.indegree_centrality;
                        vertex_temp.outdegreeCentralityValue = vertFactors.outdegree_centrality;
                        vertex_temp.influenceRangeValue = vertFactors.influence_range;
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
                        vertex_temp.indegreeCentralityValue = vertFactors.indegree_centrality;
                        vertex_temp.outdegreeCentralityValue = vertFactors.outdegree_centrality;
                        vertex_temp.influenceRangeValue = vertFactors.influence_range;
                    }

                    List<int> edges = new List<int>();
                    if (vertex_info.LinkDb != null)
                        foreach (var link_containing in vertex_info.LinkDb.Where( linkk => linkk.network_id == searched.id))
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
        //popraw - j.w.
        public Network MakeNetworkFromDb(int id, string date, int vertid, int incl)
        {
            var searched = db.NetworkDb.ToList().Find(net => net.id == id);
            DateTime dateT = DateTime.Parse(date);
            Network network_temp = new Network //dane o sieci
            {
                id = (int)searched.id,
                serviceType = searched.service_id,
                name = searched.name
            };
            List<Link> links = new List<Link>(); //tworzenie połączeń
            foreach (var linkdb in db.LinkDb.Where(n => n.network_id == searched.id && n.date_modified == dateT && (n.source_id == vertid || n.target_id == vertid))) //połączenia o początku lub końcu w podanym wierzchołku
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
                    var vertFactors = db.VertexFactorsDb.ToList().SingleOrDefault(v => (v.vertex_id == vertex_info.id && v.date == dateT && v.up_to_date));
                    if (vertFactors != null)
                    {
                        vertex_temp.betweennessCentralityValue = (float)vertFactors.betweenness_centrality;
                        vertex_temp.closenessCentralityValue = (float)vertFactors.closeness_centrality;
                        vertex_temp.indegreeCentralityValue = vertFactors.indegree_centrality;
                        vertex_temp.outdegreeCentralityValue = vertFactors.outdegree_centrality;
                        vertex_temp.influenceRangeValue = vertFactors.influence_range;
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
                        vertex_temp.indegreeCentralityValue = vertFactors.indegree_centrality;
                        vertex_temp.outdegreeCentralityValue = vertFactors.outdegree_centrality;
                        vertex_temp.influenceRangeValue = vertFactors.influence_range;
                    }

                    List<int> edges = new List<int>();
                    if (vertex_info.LinkDb != null)
                        foreach (var link_containing in vertex_info.LinkDb)
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
        [System.Web.Http.HttpGet]
        public JsonResult<Network> GetNetwork(int id, string date, int incl, int vertid)
        {
            Network net = new Network();
            net = vertid == 0 ? MakeNetworkFromDb(id, date, incl) : MakeNetworkFromDb(id, date, vertid, incl);
            return Json(net);
            
        }

        [System.Web.Http.HttpGet]
        //raczej niepotrzebne i można usunąć
        public JsonResult<Network> GetNetworkPartial(int id, string date, int vertid, int incl)
        {
            Network net = MakeNetworkFromDb(id, date, vertid, incl);
            return Json(net);

        }

        public class NetworkList
        {
            public string name { get; set; }
            public int id { get; set; }
            public List<DateTime> datesOfUpdates { get; set; }
            public List<VertexN> vertices { get; set; }
        }

        public class VertexN
        {
            public string name { get; set; }
            public int id { get; set; }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/apinetwork/getlistofnetworks")]
        [System.Web.Http.Route("Visualisation/api/apinetwork/getlistofnetworks")]
        public JsonResult<List<NetworkList>> GetListOfNetworks()
        {
            List<NetworkList> networks = new List<NetworkList>();
            foreach (var i in db.NetworkDb)
            {
                NetworkList net = new NetworkList()
                {
                    name = i.name,
                    id = (int)i.id,
                    datesOfUpdates = new List<DateTime>()
                };
                var distinctDates = i.LinkDb.Select(p => p.date_modified).Distinct().ToList();
                net.datesOfUpdates = distinctDates;
                if (distinctDates.Count == 0)
                    net.datesOfUpdates = new List<DateTime> { i.date_created };
                List<VertexN> vertices = new List<VertexN>();
                foreach (var link in i.LinkDb)
                {
                    if (vertices.All(v => v.id != link.VertexDb.id))
                    {
                        VertexN vert = new VertexN { id = (int)link.VertexDb.id, name = link.VertexDb.name };
                        vertices.Add(vert);
                    }
                    if (vertices.All(v => v.id != link.VertexDb1.id))
                    {
                        VertexN vert = new VertexN { id = (int)link.VertexDb1.id, name = link.VertexDb1.name };
                        vertices.Add(vert);
                    }
                }
                net.vertices = vertices;
                networks.Add(net);
            }
            return Json(networks);
        }

        [System.Web.Http.HttpPost]
        //nie działa obecnie
        public IHttpActionResult SaveData([FromBody] Vertex vert, int id)
        {
            Network networkTemp = MakeNetworkFromDb(id, "dd", 0);
            if (networkTemp.vertices.Any(vertex => vertex.id == vert.id)) //jeśli istnieje - usuwa
            {
                var vertexToDelete = networkTemp.vertices.Find(vertex => vertex.id == vert.id);
                networkTemp.vertices.Remove(vertexToDelete);
                networkTemp.links.RemoveAll(link => (link.source == vert.id));

            }

            bool flag = true;
            foreach (int e in vert.edges)
                if (networkTemp.vertices.All(vertex => vertex.id != e))
                    flag = false;
            if (flag)
            {
                networkTemp.vertices.Add(vert);
                foreach (int e in vert.edges)
                {
                    Link a = new Link() { source = vert.id, target = e };
                    networkTemp.links.Add(a);
                }
                return Ok();
            }
            return BadRequest("Nie istnieją wierzchołki, do których chcesz stworzyć krawędzie");
        }

        [System.Web.Http.HttpPost]
        //nie działa obecnie
        public IHttpActionResult Delete([FromBody] string index, int id)
        {
            string path = HostingEnvironment.MapPath("~/Graphs/" + id + ".json");
            Network networkTemp = JsonConvert.DeserializeObject<Network>(File.ReadAllText(path));
            string message;
            try
            {
                int indexInt = Int32.Parse(index);
                Vertex searchVertex = networkTemp.vertices.Find(vert => vert.id == indexInt);
                if (networkTemp.vertices.Remove(searchVertex))
                {

                    networkTemp.links.RemoveAll(link => (link.source == indexInt || link.target == indexInt));

                    foreach (Vertex v in networkTemp.vertices)
                    {
                        v.edges.RemoveAll(edge => edge == indexInt);
                    }
                    File.WriteAllText(path, JsonConvert.SerializeObject(networkTemp, Formatting.Indented));
                    return Ok();
                }
                else
                {
                    message = "podany wierzchołek nie istnieje w sieci";
                    return BadRequest(message);
                }
            }
            catch (ArgumentNullException)
            {
                message = "błędny format liczbowy";
                return BadRequest(message);
            }
            catch (OverflowException)
            {
                message = "za duża liczba";
                return BadRequest(message);
            }
        }

        [System.Web.Http.HttpPost]
        //nie działa obecnie
        public IHttpActionResult CreateNew([FromBody] string name)
        {
            string[] jsonFiles = Directory.GetFiles((HttpContext.Current.Server.MapPath("~/Graphs/")), "*.json")
                         .Select(Path.GetFileNameWithoutExtension)
                         .ToArray();
            foreach (string s in jsonFiles)
            {
                int n;
                if (Int32.TryParse(s, out n)) ;


            }

            int[] b = new int[jsonFiles.Length];

            for (int i = 0; i < jsonFiles.Length; i++)
            {
                int c;
                if (Int32.TryParse(jsonFiles[i], out c))
                    b[i] = c;
            }
            int nextId = b.Max() + 1;
            Network newNet = new Network { name = name, id = nextId, links = new List<Link>(), vertices = new List<Vertex>() };
            Vertex initVer = new Vertex { id = 1, edges = new List<int>() };
            newNet.vertices.Add(initVer);
            string path = HostingEnvironment.MapPath("~/Graphs/" + nextId + ".json");
            File.WriteAllText(path, JsonConvert.SerializeObject(newNet, Formatting.Indented));

            return Ok();
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult Count(int id, string date, int incl)
        {
            Network networkTemp = MakeNetworkFromDb(id, date, 1); // w tym momencie liczy zawsze z wierzchołkiem początkowym
            DateTime dateT = DateTime.Parse(date);
            networkTemp.ClosenessCentrality2();
            networkTemp.InfluenceRange2();
            networkTemp.CentralityIn2();
            networkTemp.CentralityOut2();
            networkTemp.BetweennessCentrality2();
            networkTemp.Normalize();
            foreach (Vertex vert in networkTemp.vertices)
            {
                //up to date - aplikacja aktulnie zapisuje nowy wiersz, a stary oznacza jako nieaktualny; docelowo lepiej zmienić na aktualizowanie starego
                var previous = db.VertexFactorsDb.SingleOrDefault(o => o.vertex_id == vert.id && o.date == dateT && o.up_to_date);
                if (previous != null)
                {
                    previous.up_to_date = false;
                }

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
            var previousN = db.NetworkFactorsDb.SingleOrDefault(o => o.network_id == networkTemp.id && o.date == dateT && o.up_to_date);
            if (previousN != null)
            {
                previousN.up_to_date = false;
            }

            var rowN = new NetworkFactorsDb
            {
                network_id = id,
                av_indegree_centrality = networkTemp.AverageFactor(1),
                av_outdegree_centrality = networkTemp.AverageFactor(2),
                av_closeness_centrality = networkTemp.AverageFactor(3),
                av_betweenness_centrality = networkTemp.AverageFactor(4),
                av_influence_range = networkTemp.AverageFactor(5),
                density = networkTemp.Density(),
                date = DateTime.Parse(date),
                up_to_date = true
            };
            db.NetworkFactorsDb.Add(rowN);
            db.SaveChanges();


            return Ok();
        }

        [System.Web.Http.HttpGet]
        public JsonResult<NetworkFactors> GetFactors(int id, string date)
        {
            NetworkFactors ans = new NetworkFactors();
            DateTime dateT = DateTime.Parse(date);
            if (db.NetworkFactorsDb.Any(row => row.network_id == id && row.date == dateT && row.up_to_date))
            {
                var row = db.NetworkFactorsDb.ToList().Find(v => v.network_id == id && v.date == dateT && v.up_to_date);
                ans.status = "";
                ans.avBetCen = (float)row.av_betweenness_centrality;
                ans.avCloCen = (float)row.av_closeness_centrality;
                ans.avInCen = (float)row.av_indegree_centrality;
                ans.avInfRan = (float)row.av_influence_range;
                ans.avOutCen = (float)row.av_outdegree_centrality;
                ans.density = (float)row.density;
            }
            else if (db.NetworkFactorsDb.Any(row => row.network_id == id && row.date == dateT && !row.up_to_date))
            {
                var row = db.NetworkFactorsDb.ToList().Find(v => v.network_id == id && v.date == dateT && !v.up_to_date);  //pewnie nie działa, gdy jest więcej nieaktualnych wyników
                ans.status = "Dane są nieaktualne";
                ans.avBetCen = (float)row.av_betweenness_centrality;
                ans.avCloCen = (float)row.av_closeness_centrality;
                ans.avInCen = (float)row.av_indegree_centrality;
                ans.avInfRan = (float)row.av_influence_range;
                ans.avOutCen = (float)row.av_outdegree_centrality;
                ans.density = (float)row.density;
            }
            else
            {
                ans.status = "Nie obliczono jeszcze współczynników dla tej sieci";
            }
            return Json(ans);
        }
    }
}
