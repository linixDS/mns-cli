using Newtonsoft.Json;
using System;

namespace MikroTik.Types
{
    public class mtCacheWiFi
    {
        public string InterfaceName { get; set; }
        public string SSID { get; set; }

        public object HandleCAP { get; set; }

        public mtCacheWiFi(string name, string ssid)
        {
            InterfaceName = name;
            SSID = ssid;
            HandleCAP = null;
        }

        public mtCacheWiFi(string name, string ssid, object handle)
        {
            InterfaceName = name;
            SSID = ssid;
            HandleCAP = handle;
        }

    }

    public class mtErrorMessage
    {
        [JsonProperty("error")]
        public int ErrorCode {get; set;}

        [JsonProperty("message")]
        public string Message {get; set;}

        [JsonProperty("detail")]
        public string Detail { get; set; }

        public mtErrorMessage()
        {
            ErrorCode = 0;
            Message = String.Empty;
            Detail = String.Empty;
        }

        public string GetDetailMessage()
        {
            return Message + ": " + Detail;
        }

    }

    public class mtNewScheduler
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("disabled")]
        public string Disabled { get; set; }

        [JsonProperty("interval")]
        public string Intervaal { get; set; }

        [JsonProperty("on-event")]
        public string OnEvent { get; set; }


        [JsonProperty("start-date")]
        public string StartDate { get; set; }

        [JsonProperty("start-time")]
        public string StartTime { get; set; }


        public mtNewScheduler()
        {
            Name = String.Empty;
            Disabled = "false";
            StartDate = String.Empty;
            StartTime = String.Empty;
            OnEvent = String.Empty;
            Intervaal= String.Empty;
        }
    }


    public class mtSchedulerInfo
    {
        [JsonProperty(".id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("disabled")]
        public bool  Disabled { get; set; }
        
        [JsonProperty("interval")]
        public string Intervaal { get; set; }
        
        [JsonProperty("next-run")]
        public string NextRun { get; set; }
        
        [JsonProperty("on-event")]
        public string OnEvent { get; set; }
       
        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("policy")]
        public string Policy { get; set; }

        [JsonProperty("run-count")]
        public string RunCount { get; set; }


        [JsonProperty("start-date")]
        public string StartDate { get; set; }

        [JsonProperty("start-time")]
        public string StartTime { get; set; }


        [JsonProperty("comment")]
        public string Comment { get; set; }

        public mtSchedulerInfo()
        {
            Comment = String.Empty;
            Name= String.Empty;
            Disabled = false;
            Id  = String.Empty;
            Intervaal= String.Empty;
            NextRun= String.Empty;
            StartDate= String.Empty;
            StartTime= String.Empty;
            RunCount= String.Empty;
            OnEvent= String.Empty;
            Owner= String.Empty;
            Policy= String.Empty;
        }
    }


    public class mtScriptInfo
    {
        [JsonProperty(".id")]
        public string Id { get; set; }
        [JsonProperty(".nextid")]
        public string NextID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("dont-require-permissions")]
        public string DontRequirePermissions { get; set; }

        [JsonProperty("last-started")]
        public string LastStartedTime { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }
        [JsonProperty("Policy")]
        public string Policy { get; set; }
        [JsonProperty("run-count")]
        public string RunCount { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        public mtScriptInfo()
        {
            Id= String.Empty;
            NextID  = String.Empty;
            Name= String.Empty;
            DontRequirePermissions= String.Empty;
            LastStartedTime= String.Empty;
            Owner= String.Empty;
            Policy= String.Empty;
            RunCount= String.Empty;
            Comment= String.Empty;
            Source = String.Empty;
        }
    }

    public class mtRequestDataId
    {
        
        [JsonProperty(".id")]
        public string Id {get; set;}
        public mtRequestDataId(String _id)
        {
            Id = _id;
        }
    }

    public class mtPackageInfo
    {
        [JsonProperty(".id")]
        public string Id {get; set;}

        [JsonProperty("build-time")]
        public string buildTime {get; set;}

        [JsonProperty("disabled")]
        public string Disabled {get; set;}

        [JsonProperty("name")]
        public string Name {get; set;}

        [JsonProperty("scheduled")]
        public string Scheduled {get; set;}

        [JsonProperty("version")]
        public string Version {get; set;}

        public  mtPackageInfo()
        {
            Id = "";
            Name = "";
            Disabled = "";
            buildTime = "";
            Version = "";
            Scheduled = "";
        }
    }


    public class mtCAPsMANConfigurationInfo
    {
        [JsonProperty(".id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("ssid")]
        public string SSID { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }
        [JsonProperty("channel.tx-power")]
        public string TxPower { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("installation")]
        public string Installation { get; set; }
        [JsonProperty("multicast-helper")]
        public string MulticastHelper { get; set; }
        [JsonProperty("rx-chains")]
        public string RxChains { get; set; }
        [JsonProperty("tx-chains")]
        public string TxChains { get; set; }
        [JsonProperty("security")]
        public string Security { get; set; }

        [JsonProperty("datapath")]
        public string DataPath { get; set; }
        public mtCAPsMANConfigurationInfo()
        {
            Id = String.Empty;
            Name = String.Empty;
            SSID = String.Empty;
            TxPower = String.Empty;
            TxChains = String.Empty;
            RxChains = String.Empty;
            DataPath = String.Empty;
            Installation = String.Empty;
            Channel = String.Empty;
            Comment = String.Empty;
            Country = String.Empty;
            MulticastHelper = String.Empty;
            Security = String.Empty;
        }
    }





    public class mtCAPsMANRegistrationTableInfo
    {
        [JsonProperty(".id")]
        public string Id { get; set; }
        [JsonProperty("comment")]
        public string Comment {get; set;}
        [JsonProperty("interface")]
        public string Interface {get; set;}        
        [JsonProperty("mac-address")]
        public string MAC {get; set;} 
        [JsonProperty("ssid")]
        public string SSID {get; set;}                 
        [JsonProperty("rx-rate")]
        public string RxRate {get; set;}          
        [JsonProperty("rx-signal")]
        public string RxSignal {get; set;}
        [JsonProperty("tx-rate")]
        public string TxRate {get; set;}   
        [JsonProperty("uptime")]
        public string Uptime {get; set;}                        
        [JsonProperty("packets")]
        public string Packets {get; set;}      
        [JsonProperty("bytes")]
        public string Bytes {get; set;}               

        public mtCAPsMANRegistrationTableInfo()
        {
            Id = "";
            Comment = "";
            Interface = "";
            MAC = "";
            SSID = "";
            RxRate = "";
            RxSignal = "";
            TxRate = "";
            Uptime = "";
            Packets = ""; 
            Bytes = "";  
        }

        public string GetBytes()
        {
            try
            {
                string[] values = this.Bytes.Split(',');
                if (values.Length == 2)
                {
                    int[] size = new int[2];
                    string[] _bytes = new string[2];

                    size[0] = Convert.ToInt32(values[0]);
                    size[1] = Convert.ToInt32(values[1]);

                    if (size[0] > 1024)
                    {
                        size[0] = size[0] / 1024;
                        _bytes[0] = size[0].ToString()+" kb";
                    }
                        else
                        _bytes[0] = size[0].ToString() + " kb";

                    if (size[1] > 1024)
                    {
                        size[1] = size[1] / 1024;
                        _bytes[1] = size[1].ToString() + " kb";
                    }
                    else
                        _bytes[1] = size[1].ToString() + " kb";


                    return String.Format("{0}/{1}", _bytes[0], _bytes[1]);

                }
                else
                    return this.Bytes;
            }
            catch (Exception)
            {
                return this.Bytes;
            }



        }
    }

    public class mtCAPsMANRemoteCAPInfo
    {
        [JsonProperty(".id")]
        public string Id {get; set;}

       [JsonProperty("address")]
        public string Address {get; set;}
       [JsonProperty("base-mac")]
        public string MAC {get; set;}        
       [JsonProperty("board")]
        public string Board {get; set;} 
       [JsonProperty("identity")]
        public string Identity {get; set;}               
       [JsonProperty("name")]
        public string Name {get; set;}        
       [JsonProperty("radios")]
        public string Radios {get; set;}        
       [JsonProperty("serial")]
        public string Serial {get; set;}        
       [JsonProperty("state")]
        public string State {get; set;} 
       [JsonProperty("version")]
        public string Version {get; set;}  

        public mtCAPsMANRemoteCAPInfo()
        {
            Id = "";
            Version = "";
            Address = "";
            MAC = "";
            Board = "";
            Identity = "";
            Name = "";
            Radios = "";
            Serial = "";
            State = "";
        }             
    }


    public class mtCAPsMANInterfaceInfo
    {
        [JsonProperty("comment")]
        public string Comment {get; set;}
        [JsonProperty("name")]
        public string Name {get; set;}

        [JsonProperty("channel-frequency")]
        public string ChannelFrequency {get; set;}
        [JsonProperty("configuration")]
        public string Configuration {get; set;}
        [JsonProperty("current-autorized-clients")]
        public string CurrentAuthClients {get; set;}
        [JsonProperty("current-channel")]
        public string CurrentChannel {get; set;}
        [JsonProperty("current-rate-set")]
        public string CurrentRateSet {get; set;}
        [JsonProperty("current-registered-clients")]
        public string CurrentRegisteredClients {get; set;}
        [JsonProperty("current-state")]
        public string CurrentState {get; set;}
        [JsonProperty("disabled")]
        public string Disabled {get; set;}
        [JsonProperty("inactive")]
        public string Inactive {get; set;}
        [JsonProperty("l2mtu")]
        public string L2MTU {get; set;}
        [JsonProperty("mac-address")]
        public string MAC {get; set;}
        [JsonProperty("master")]
        public string Master {get; set;}
        [JsonProperty("master-interface")]
        public string MasterInterface {get; set;}
        [JsonProperty("datapath")]
        public string DataPath {get; set;}

        [JsonProperty("security")]
        public string Security {get; set;}

        [JsonProperty("radio-name")]
        public string RadioName {get; set;}
        [JsonProperty("radio-mac")]
        public string RadioMAC {get; set;}


        public mtCAPsMANInterfaceInfo()
        {
            Comment = "";
            Name = "";
            ChannelFrequency = "";
            Configuration = "";
            CurrentAuthClients = "";
            CurrentChannel = "";
            CurrentRateSet = "";
            CurrentRegisteredClients = "";
            Disabled = "";
            CurrentState = "";
            Inactive = "";
            L2MTU = "";
            MAC = "";
            Master = "";
            MasterInterface = "";
            DataPath = "";
            Security = "";
            RadioName = "";
            RadioMAC = "";
        }
    }


    public class mtInterfaceInfo
    {
        [JsonProperty("comment")]
        public string Comment {get; set;}
        [JsonProperty("mac-address")]
        public string MAC {get; set;}
        [JsonProperty("name")]
        public string Name {get; set;}
        [JsonProperty("disabled")]
        public string Disabled {get; set;}
        [JsonProperty("type")]
        public string Type {get; set;}
        [JsonProperty("mtu")]
        public string MTU {get; set;}  
        [JsonProperty("link-downs")]
        public string LinkDown {get; set;}  

        public mtInterfaceInfo()
        {
            Name = "";
            LinkDown = "";
            Type = "";
            MAC = "";
            Comment = "";
            Disabled = "";
            MTU = "";
        }    
    }

    public class mtEthernetInfo
    {
  
        [JsonProperty("default-name")]
        public string DefaultName {get; set;}
        [JsonProperty("mac-address")]
        public string MAC {get; set;}
        [JsonProperty("name")]
        public string Name {get; set;}
        [JsonProperty("disabled")]
        public string Disabled {get; set;}
        [JsonProperty("mtu")]
        public string MTU {get; set;}  
        [JsonProperty("comment")]
        public string Comment {get; set;}  
        [JsonProperty("full-duplex")]
        public string FullDuplex {get; set;}                  
        [JsonProperty("speed")]
        public string Speed {get; set;}   

        public mtEthernetInfo()
        {
            Speed = "";
            FullDuplex = "";
            MTU = "";
            Disabled = "";
            Name = "";
            MAC = "";
            DefaultName = "";
            Comment = "";
        }                       
    }


    public class mtNeighborInfo
    {
        [JsonProperty("address")]
        public string AddressIP {get; set;}
      [JsonProperty("address4")]
        public string AddressIPv4 {get; set;}

        [JsonProperty("age")] 
        public string Age {get; set;}
        [JsonProperty("board")] 
        public string Board {get; set;}
        [JsonProperty("identity")] 
        public string Identity {get; set;}    
        [JsonProperty("interface")]    
        public string Interface {get; set;}
        [JsonProperty("interface-name")]    
        public string InterfaceName {get; set;}        
        [JsonProperty("mac-addresss")]    
        public string MAC {get; set;}  
        [JsonProperty("platform")] 
        public string Platform {get; set;}          
        [JsonProperty("software-id")]    
        public string SoftwareID {get; set;}  
        [JsonProperty("system-caps")]    
        public string SystemCAPS {get; set;}

        [JsonProperty("system-caps-enabled")]    
        public string SystemCAPSEnabled {get; set;}            

        [JsonProperty("system-description")]    
        public string SystemDescription {get; set;} 
        [JsonProperty("unpack")]    
        public string Unpack {get; set;}           
        [JsonProperty("uptime")]    
        public string Uptime {get; set;}   
        [JsonProperty("version")]    
        public string Version {get; set;}  

        public mtNeighborInfo()
        {
            Version = "";
            AddressIP = "";
            AddressIPv4 = "";
            Age = "";
            Board = "";
            MAC = "";
            Platform = "";
            Identity = "";
            Interface = "";
            InterfaceName = "";
            SoftwareID = "";
            SystemCAPS = "";
            SystemCAPSEnabled = "";
            SystemDescription = "";
            Unpack = "";
            Uptime = "";
        }         
    }

    public class mtUserManagerInfo
    {
        [JsonProperty("name")]
        public string MACAddress {get; set;}
        
        [JsonProperty("disabled")]        
        public string Disabled {get; set;}
        [JsonProperty("password")] 
        public string Password {get; set;}  
        
        [JsonProperty("group")] 
        public string Group {get; set;}
        [JsonProperty("otp-secret")] 
        public string OTPSecret {get; set;}
  
        [JsonProperty("shared-users")]    
        public string SharedUsers {get; set;}
        [JsonProperty("attributes")]    
        public string Attributes {get; set;}        
 

        public mtUserManagerInfo(){
            MACAddress = "";
            Disabled = "";
            Password = "";
            Group = "";
            OTPSecret = "";
            SharedUsers = "";
            Attributes = "";
        }         
    }


    public class mtRequestNewUserData
    {


        [JsonProperty("name")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }
        [JsonProperty("attributes")]
        public string Attributes { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("disabled")]
        public string Disabled { get; set; }


        public mtRequestNewUserData()
        {
            UserName = "";
            Password = "";
            Group = "default";
            Attributes = "";
            Disabled = "false";
            Comment = "";
        }
    }

}