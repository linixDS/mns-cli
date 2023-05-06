using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using FortiOS.Types;
using CacheManagment;


namespace FortiOS.Client
{
    public class FortiClient
    {
        protected String IPAddress;
        protected int Port;
        protected String TokenAPI;
        protected String Url;
        protected HttpClient client;
        protected String LastError;
        protected bool callEvents = true;

        public delegate void onEventError(string message);
        public delegate void onEventRequest(string url, object data);
        public delegate void onEventResponde(int statusCode, object data);
        public delegate void onEventSuccess(object data);

        public event onEventRequest OnEventRequest;
        public event onEventError OnEventError;
        public event onEventResponde OnEventResponde;
        public event onEventSuccess OnEventSuccess;


        public FortiClient(string ip, int port, string token)
        {
            try
            {
                IPAddress = ip;
                Port = port;
                TokenAPI = token;

                Url = String.Format("https://{0}:{1}/api/v2/", IPAddress, Port);
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                };

                client = new HttpClient(httpClientHandler);
                client.Timeout = TimeSpan.FromSeconds(15);
            }
            catch (Exception error)
            {
                Console.WriteLine("EXCEPTION: " + error.Message);
            }
        }

        public string GetLastError()
        {
            return LastError;
        }

        private async Task<FortiRespondeRestApi> SendGet(string url)
        {
            LastError = "";
            HttpResponseMessage response;
            FortiRespondeRestApi res;

            var cache = new DataCache();
            cache.LoadConfig();
            var cacheData = cache.Find(url);
            if (cacheData != null)
            {
                if (!cache.IsRenewCache(cacheData))
                {
                    var value = cache.GetCacheValue(cacheData);
                    if (value != null)
                    {
                        res = new FortiRespondeRestApi(HttpStatusCode.OK, value);
                        return res;
                    }

                }
            }

            try
            {
                Debug.WriteLine("GET " + url);
                if (OnEventRequest != null && callEvents)
                    OnEventRequest(url, null);

                response = await client.GetAsync(url);

                Debug.WriteLine(" -> RESPONDE CODE:" + (int)response.StatusCode);
                var json = await response.Content.ReadAsStringAsync();
                
                res = new FortiRespondeRestApi(response.StatusCode, json);
                return res;
            }
            catch (Exception e)
            {
                LastError = "Error: " + e.Message;
                if (OnEventError != null && callEvents)
                    OnEventError(LastError);
            }

            return null;
        }

        /*---------------------------------------------------------------------------------------------------------------
         *GetAllDevicesFromNetworkAsync
         *---------------------------------------------------------------------------------------------------------------*/

        public async Task<(bool,object)> GetAllDevicesFromNetworkAsync()
        {
            string url = String.Format("{0}monitor/user/device/query?access_token={1}", this.Url, this.TokenAPI);
            LastError = "";
            try
            {
                FortiRespondeRestApi responde = await SendGet(url);
                if (responde == null) return (false, null);
                
                if ((int)responde.statusCode == 200)
                {
                    
                    var data = JsonSerializer.Deserialize<FortiResultNetworkDevices>(responde.data.ToString());

                    if (OnEventResponde != null && callEvents)
                    {
                        OnEventResponde((int)responde.statusCode, data);
                    }

                    if (OnEventSuccess != null && callEvents)
                    {
                        OnEventSuccess(data);
                    }

                    return (true, data);
                }
                    else
                {
                    LastError = String.Format("Error code {0}: {1}",(int)responde.statusCode, responde.statusCode.ToString());
                    if (OnEventError != null && callEvents)
                        OnEventError(LastError);
                    return (false, null);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e.Message);
                LastError = e.Message;
                if (OnEventError != null && callEvents)
                    OnEventError(e.Message);
            }

            return (false,null);
        }


        public List<FortiRegisterDevice> GetAllDevicesFromNetwork()
        {
            bool res = false;
            object data;
            try
            {
                callEvents = false;

                var t = Task.Run(() =>
                {
                    return GetAllDevicesFromNetworkAsync();
                });
                (res, data) = t.Result;

                callEvents = true;
                if (res)
                {
                    FortiResultNetworkDevices result = (FortiResultNetworkDevices)data;
                    return result.clients;
                }
            }
            catch (Exception e)
            {
                callEvents = true;
                 LastError = "Error:" + e.Message;
            }

            return null;
        }


        /*---------------------------------------------------------------------------------------------------------------
         *GetDHCPLeasesFromNetworkAsync
         *---------------------------------------------------------------------------------------------------------------*/

        public async Task<(bool, object)> GetDHCPLeasesFromNetworkAsync()
        {
            string url = String.Format("{0}monitor/system/dhcp?access_token={1}", this.Url, this.TokenAPI);
            LastError = "";
            try
            {
                FortiRespondeRestApi responde = await SendGet(url);
                if (responde == null) return (false, null);


                if ((int)responde.statusCode == 200)
                {
                    var data = JsonSerializer.Deserialize<FortiResultDHCPLease>(responde.data.ToString());
                    if (OnEventResponde != null && callEvents)
                    {
                        OnEventResponde((int)responde.statusCode, data);
                    }

                    if (OnEventSuccess != null && callEvents)
                    {
                        OnEventSuccess(data);
                    }

                    return (true, data);
                }
                else
                {
                    LastError = String.Format("Error code {0}: {1}", (int)responde.statusCode, responde.statusCode.ToString());
                    if (OnEventError != null && callEvents)
                        OnEventError(LastError);
                    return (false, null);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e.Message);
                LastError = e.Message;
                if (OnEventError != null && callEvents)
                    OnEventError(e.Message);
            }

            return (false, null);
        }


        public List<FortiDHCPLease> GetDHCPLeasesFromNetwork()
        {
            bool res = false;
            object data;
            try
            {
                callEvents = false;

                var t = Task.Run(() =>
                {
                    return GetDHCPLeasesFromNetworkAsync();
                });
                (res, data) = t.Result;

                callEvents = true;
                if (res)
                {
                    FortiResultDHCPLease result = (FortiResultDHCPLease)data;
                    return result.clients;
                }
            }
            catch (Exception e)
            {
                callEvents = true;
               
                LastError = "Error:" + e.Message;
            }

            return null;
        }



        /*---------------------------------------------------------------------------------------------------------------
         *GetDHCPLeasesFromNetworkAsync
         *---------------------------------------------------------------------------------------------------------------*/

        public async Task<(bool, object)> GetAllUsersFromVPNAsync()
        {
            string url = String.Format("{0}cmdb/user/local?access_token={1}", this.Url, this.TokenAPI);
            
            LastError = "";
            try
            {
                FortiRespondeRestApi responde = await SendGet(url);
                if (responde == null) return (false, null);


                if ((int)responde.statusCode == 200)
                {
                    var data = JsonSerializer.Deserialize<FortiResultVPNUsers>(responde.data.ToString());
                    if (OnEventResponde != null && callEvents)
                    {
                        OnEventResponde((int)responde.statusCode, data);
                    }

                    if (OnEventSuccess != null && callEvents)
                    {
                        OnEventSuccess(data);
                    }

                    return (true, data);
                }
                else
                {
                    LastError = String.Format("Error code {0}: {1}", (int)responde.statusCode, responde.statusCode.ToString());
                    if (OnEventError != null && callEvents)
                        OnEventError(LastError);
                    return (false, null);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e.Message);
                LastError = e.Message;
                if (OnEventError != null && callEvents)
                    OnEventError(e.Message);
            }

            return (false, null);
        }


        public List<FortiVPNUser> GetAllUsersFromVPN()
        {
            bool res = false;
            object data;
            try
            {
                callEvents = false;

                var t = Task.Run(() =>
                {
                    return GetAllUsersFromVPNAsync();
                });
                (res, data) = t.Result;

                callEvents = true;
                if (res)
                {
                    FortiResultVPNUsers result = (FortiResultVPNUsers)data;
                    return result.users;
                }
            }
            catch (Exception e)
            {
                callEvents = true;

                LastError = "Error:" + e.Message;
            }

            return null;
        }



        /*---------------------------------------------------------------------------------------------------------------
         *GetDHCPServers
         *---------------------------------------------------------------------------------------------------------------*/

        public async Task<(bool, object)> GetDHCPServersAsync()
        {
            string url = String.Format("{0}cmdb/system.dhcp/server?access_token={1}", this.Url, this.TokenAPI);

            LastError = "";
            try
            {
                FortiRespondeRestApi responde = await SendGet(url);
                if (responde == null) return (false, null);


                if ((int)responde.statusCode == 200)
                {
                    var data = JsonSerializer.Deserialize<FortiResultDHCPServers>(responde.data.ToString());
                    if (OnEventResponde != null && callEvents)
                    {
                        OnEventResponde((int)responde.statusCode, data);
                    }

                    if (OnEventSuccess != null && callEvents)
                    {
                        OnEventSuccess(data);
                    }

                    return (true, data);
                }
                else
                {
                    LastError = String.Format("Error code {0}: {1}", (int)responde.statusCode, responde.statusCode.ToString());
                    if (OnEventError != null && callEvents)
                        OnEventError(LastError);
                    return (false, null);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e.Message);
                LastError = e.Message;
                if (OnEventError != null && callEvents)
                    OnEventError(e.Message);
            }

            return (false, null);
        }


        public List<FortiDHCPServer> GetDHCPServers()
        {
            bool res = false;
            object data;
            try
            {
                callEvents = false;

                var t = Task.Run(() =>
                {
                    return GetDHCPServersAsync();
                });
                (res, data) = t.Result;

                callEvents = true;
                if (res)
                {
                    FortiResultDHCPServers result = (FortiResultDHCPServers)data;
                    return result.servers;
                }
            }
            catch (Exception e)
            {
                callEvents = true;

                LastError = "Error:" + e.Message;
            }

            return null;
        }



    }


    public class FortiRespondeRestApi
    {
        public HttpStatusCode statusCode { get; set; }
        public string data { get; set; }

        public FortiRespondeRestApi(HttpStatusCode _status, string _data)
        {
            statusCode = _status;
            data = _data;
        }
    }
}
