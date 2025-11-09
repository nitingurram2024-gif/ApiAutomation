using RestSharp;
using ApiAutomation.Base;

namespace ApiAutomation.Endpoints
{
    public class ObjectEndpoint : BaseApi
    {
        private const string Resource = "/objects";

        public IRestResponse CreateObject(object payload) => Execute(Resource, Method.POST, payload);
        public IRestResponse GetObject(string id) => Execute($"{Resource}/{id}", Method.GET, null);
        public IRestResponse UpdateObject(string id, object payload) => Execute($"{Resource}/{id}", Method.PUT, payload);
        public IRestResponse PatchObject(string id, object payload) => Execute($"{Resource}/{id}", Method.PATCH, payload);
        public IRestResponse DeleteObject(string id) => Execute($"{Resource}/{id}", Method.DELETE, null);
    }
}
