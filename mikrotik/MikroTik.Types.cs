using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace MikroTik.Types
{

    public class mtRadiusRouter
    {
        [JsonPropertyName(".id")]
        public string Id { get; set; }
        [JsonPropertyName("disabled")]
        public string Disabled { get; set; }

        [JsonPropertyName("coa-port")]
        public string COAPort { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("shared-secret")]
        public string SharedSecret { get; set; }

        public mtRadiusRouter()
        {
            Id = "";
            Disabled = "false";
            Name = "";
            SharedSecret = "";
            Address = "";
            COAPort = "";
        }
    }

    public class mtRadiusService
    {
        [JsonPropertyName(".id")]
        public string Id { get; set; }
        [JsonPropertyName("disabled")]
        public string Disabled { get; set; }

        [JsonPropertyName("service")]
        public string ServiceName { get; set; }
        [JsonPropertyName("secret")]
        public string Secret { get; set; }
        [JsonPropertyName("accounting-backup")]
        public string AccountingBackup { get; set; }
        [JsonPropertyName("accounting-port")]
        public string AccountingPort { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("authentication-port")]
        public string AuthenticationPort { get; set; }
        [JsonPropertyName("called-id")]
        public string CalledID { get; set; }
        [JsonPropertyName("certificate")]
        public string Certificate { get; set; }
        [JsonPropertyName("comment")]
        public string Comment { get; set; }
        [JsonPropertyName("domain")]
        public string Domain { get; set; }
        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }
        [JsonPropertyName("realm")]
        public string Realm { get; set; }
        [JsonPropertyName("timeout")]
        public string Timeout { get; set; }

        public mtRadiusService()
        {
            Id = "";
            Disabled = "false";
            ServiceName = "";
            Secret = "";
            AccountingPort = "";
            AccountingBackup = "";
            Address = "";
            AuthenticationPort = "";
            CalledID = "";
            Certificate = "";
            Comment = "";
            Domain = "";
            Protocol = "";
            Realm = "";
            Timeout = "";
        }
    }


    public class mtErrorMessage
    {
        [JsonPropertyName("error")]
        public int ErrorCode {get; set;}

        [JsonPropertyName("message")]
        public string Message {get; set;}

        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        public mtErrorMessage()
        {
            ErrorCode = 0;
            Message = String.Empty;
            Detail = String.Empty;
        }

        public mtErrorMessage(int code, string message, string detail)
        {
            ErrorCode = code;
            Message = message;
            Detail = detail;
        }


        public string GetDetailMessage()
        {
            return Message + ": " + Detail;
        }

        public string ToString()
        {
            return JsonSerializer.Serialize<mtErrorMessage>(this);
        }

    }

    public class mtNewScheduler
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("disabled")]
        public string Disabled { get; set; }

        [JsonPropertyName("interval")]
        public string Intervaal { get; set; }

        [JsonPropertyName("on-event")]
        public string OnEvent { get; set; }


        [JsonPropertyName("start-date")]
        public string StartDate { get; set; }

        [JsonPropertyName("start-time")]
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
        [JsonPropertyName(".id")]
        public string Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("disabled")]
        public bool  Disabled { get; set; }
        
        [JsonPropertyName("interval")]
        public string Intervaal { get; set; }
        
        [JsonPropertyName("next-run")]
        public string NextRun { get; set; }
        
        [JsonPropertyName("on-event")]
        public string OnEvent { get; set; }
       
        [JsonPropertyName("owner")]
        public string Owner { get; set; }

        [JsonPropertyName("policy")]
        public string Policy { get; set; }

        [JsonPropertyName("run-count")]
        public string RunCount { get; set; }


        [JsonPropertyName("start-date")]
        public string StartDate { get; set; }

        [JsonPropertyName("start-time")]
        public string StartTime { get; set; }


        [JsonPropertyName("comment")]
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
        [JsonPropertyName(".id")]
        public string Id { get; set; }
        [JsonPropertyName(".nextid")]
        public string NextID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("dont-require-permissions")]
        public string DontRequirePermissions { get; set; }

        [JsonPropertyName("last-started")]
        public string LastStarted { get; set; }

        [JsonPropertyName("owner")]
        public string Owner { get; set; }
        [JsonPropertyName("policy")]
        public string Policy { get; set; }
        [JsonPropertyName("run-count")]
        public string RunCount { get; set; }
        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        public mtScriptInfo()
        {
            Id= String.Empty;
            NextID  = String.Empty;
            Name= String.Empty;
            DontRequirePermissions= String.Empty;
            LastStarted= String.Empty;
            Owner= String.Empty;
            Policy= String.Empty;
            RunCount= String.Empty;
            Comment= String.Empty;
            Source = String.Empty;
        }
    }

    public class mtRequestDataId
    {
        
        [JsonPropertyName(".id")]
        public string Id {get; set;}
        public mtRequestDataId(String _id)
        {
            Id = _id;
        }
    }

    public class mtPackageInfo
    {
        [JsonPropertyName(".id")]
        public string Id {get; set;}

        [JsonPropertyName("build-time")]
        public string buildTime {get; set;}

        [JsonPropertyName("disabled")]
        public string Disabled {get; set;}

        [JsonPropertyName("name")]
        public string Name {get; set;}

        [JsonPropertyName("scheduled")]
        public string Scheduled {get; set;}

        [JsonPropertyName("version")]
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
        [JsonPropertyName(".id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("ssid")]
        public string SSID { get; set; }

        [JsonPropertyName("channel")]
        public string Channel { get; set; }
        [JsonPropertyName("channel.tx-power")]
        public string TxPower { get; set; }
        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("installation")]
        public string Installation { get; set; }
        [JsonPropertyName("multicast-helper")]
        public string MulticastHelper { get; set; }
        [JsonPropertyName("rx-chains")]
        public string RxChains { get; set; }
        [JsonPropertyName("tx-chains")]
        public string TxChains { get; set; }
        [JsonPropertyName("security")]
        public string Security { get; set; }

        [JsonPropertyName("datapath")]
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
        [JsonPropertyName(".id")]
        public string Id { get; set; }
        [JsonPropertyName("comment")]
        public string Comment {get; set;}
        [JsonPropertyName("interface")]
        public string Interface {get; set;}        
        [JsonPropertyName("mac-address")]
        public string MAC {get; set;} 
        [JsonPropertyName("ssid")]
        public string SSID {get; set;}                 
        [JsonPropertyName("rx-rate")]
        public string RxRate {get; set;}          
        [JsonPropertyName("rx-signal")]
        public string RxSignal {get; set;}
        [JsonPropertyName("tx-rate")]
        public string TxRate {get; set;}   
        [JsonPropertyName("uptime")]
        public string Uptime {get; set;}                        
        [JsonPropertyName("packets")]
        public string Packets {get; set;}      
        [JsonPropertyName("bytes")]
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
        [JsonPropertyName(".id")]
        public string Id {get; set;}

       [JsonPropertyName("address")]
        public string Address {get; set;}
       [JsonPropertyName("base-mac")]
        public string MAC {get; set;}        
       [JsonPropertyName("board")]
        public string Board {get; set;} 
       [JsonPropertyName("identity")]
        public string Identity {get; set;}               
       [JsonPropertyName("name")]
        public string Name {get; set;}        
       [JsonPropertyName("radios")]
        public string Radios {get; set;}        
       [JsonPropertyName("serial")]
        public string Serial {get; set;}        
       [JsonPropertyName("state")]
        public string State {get; set;} 
       [JsonPropertyName("version")]
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
        [JsonPropertyName("comment")]
        public string Comment {get; set;}
        [JsonPropertyName("name")]
        public string Name {get; set;}

        [JsonPropertyName("channel-frequency")]
        public string ChannelFrequency {get; set;}
        [JsonPropertyName("configuration")]
        public string Configuration {get; set;}
        [JsonPropertyName("current-authorized-clients")]
        public string CurrentAuthClients {get; set;}
        [JsonPropertyName("current-channel")]
        public string CurrentChannel {get; set;}
        [JsonPropertyName("current-rate-set")]
        public string CurrentRateSet {get; set;}
        [JsonPropertyName("current-registered-clients")]
        public string CurrentRegisteredClients {get; set;}
        [JsonPropertyName("current-state")]
        public string CurrentState {get; set;}
        [JsonPropertyName("disabled")]
        public string Disabled {get; set;}
        [JsonPropertyName("inactive")]
        public string Inactive {get; set;}
        [JsonPropertyName("l2mtu")]
        public string L2MTU {get; set;}
        [JsonPropertyName("mac-address")]
        public string MAC {get; set;}
        [JsonPropertyName("master")]
        public string Master {get; set;}
        [JsonPropertyName("master-interface")]
        public string MasterInterface {get; set;}


        [JsonPropertyName("radio-name")]
        public string RadioName {get; set;}
        [JsonPropertyName("radio-mac")]
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
            RadioName = "";
            RadioMAC = "";
        }
    }


    public class mtInterfaceInfo
    {
        [JsonPropertyName("comment")]
        public string Comment {get; set;}
        [JsonPropertyName("mac-address")]
        public string MAC {get; set;}
        [JsonPropertyName("name")]
        public string Name {get; set;}
        [JsonPropertyName("disabled")]
        public string Disabled {get; set;}
        [JsonPropertyName("type")]
        public string Type {get; set;}
        [JsonPropertyName("mtu")]
        public string MTU {get; set;}  
        [JsonPropertyName("link-downs")]
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
  
        [JsonPropertyName("default-name")]
        public string DefaultName {get; set;}
        [JsonPropertyName("mac-address")]
        public string MAC {get; set;}
        [JsonPropertyName("name")]
        public string Name {get; set;}
        [JsonPropertyName("disabled")]
        public string Disabled {get; set;}
        [JsonPropertyName("mtu")]
        public string MTU {get; set;}  
        [JsonPropertyName("comment")]
        public string Comment {get; set;}  
        [JsonPropertyName("full-duplex")]
        public string FullDuplex {get; set;}                  
        [JsonPropertyName("speed")]
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


    public class mtDHCPServerLease
    {

        [JsonPropertyName(".id")]
        public string Id { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("mac-address")]
        public string MAC { get; set; }
        [JsonPropertyName("host-name")]
        public string HostName { get; set; }

        [JsonPropertyName("server")]
        public string DHCPServer { get; set; }

        [JsonPropertyName("disabled")]
        public string Disabled { get; set; }
        [JsonPropertyName("dynamic")]
        public string DynamicAddress { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("last-seen")]
        public string LastSeen { get; set; }

        [JsonPropertyName("expires-after")]
        public string ExpiresAfter { get; set; }


        public mtDHCPServerLease()
        {
            Id = "";
            Address = "";
            HostName = "";
            Disabled = "";
            DHCPServer = "";
            MAC = "";
            DynamicAddress = "";
            Status = "";
            LastSeen = "";
            ExpiresAfter = "";
        }
    }


    public class mtNeighborInfo
    {
        [JsonPropertyName("address")]
        public string AddressIP {get; set;}
        [JsonPropertyName("address4")]
        public string AddressIPv4 {get; set;}

        [JsonPropertyName("age")] 
        public string Age {get; set;}
        [JsonPropertyName("board")] 
        public string Board {get; set;}

        [JsonPropertyName("identity")]
        public string Identity { get; set; }

        [JsonPropertyName("interface")]    
        public string Interface {get; set;}
        [JsonPropertyName("interface-name")]    
        public string InterfaceName {get; set;}    
            
        [JsonPropertyName("mac-address")]    
        public string MAC {get; set;}  
        [JsonPropertyName("platform")] 
        public string Platform {get; set;}          
        [JsonPropertyName("software-id")]    
        public string SoftwareID {get; set;}  
        [JsonPropertyName("system-caps")]    
        public string SystemCAPS {get; set;}

        [JsonPropertyName("system-caps-enabled")]    
        public string SystemCAPSEnabled {get; set;}            

        [JsonPropertyName("system-description")]    
        public string SystemDescription {get; set;} 
        [JsonPropertyName("unpack")]    
        public string Unpack {get; set;}           
        [JsonPropertyName("uptime")]    
        public string Uptime {get; set;}   
        [JsonPropertyName("version")]    
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
        [JsonPropertyName(".id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name {get; set;}
        
        [JsonPropertyName("disabled")]        
        public string Disabled {get; set;}
        [JsonPropertyName("password")] 
        public string Password {get; set;}  
        
        [JsonPropertyName("group")] 
        public string Group {get; set;}
        [JsonPropertyName("otp-secret")] 

        public string OTPSecret {get; set;}
  
        [JsonPropertyName("shared-users")]    
        public string SharedUsers {get; set;}
        [JsonPropertyName("attributes")]    
        public string Attributes {get; set;}
        [JsonPropertyName("comment")]
        public string Comment { get; set; }


        public mtUserManagerInfo(){
            Name = "";
            Disabled = "";
            Password = "";
            Group = "";
            OTPSecret = "";
            SharedUsers = "";
            Attributes = "";
            Comment = "";
            Id = "";
        }         
    }


    public class mtUserManagerGroup
    {

        [JsonPropertyName(".id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("attributes")]
        public string Attributes { get; set; }

        [JsonPropertyName("inner-auths")]
        public string InnerAuths { get; set; }

        [JsonPropertyName("outer-auths")]
        public string OuterAuths { get; set; }

        public mtUserManagerGroup()
        {
            Id = "";
            Name = "";
            Attributes = "";
            InnerAuths = "";
            OuterAuths = "";
        }
    }


    public class mtRequestNewUserData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("group")]
        public string Group { get; set; }
        [JsonPropertyName("attributes")]
        public string Attributes { get; set; }
        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("disabled")]
        public string Disabled { get; set; }


        public mtRequestNewUserData()
        {
            Name = "";
            Password = "";
            Group = "default";
            Attributes = "";
            Disabled = "false";
            Comment = "";
        }

        public mtRequestNewUserData(mtUserManagerInfo data)
        {
            Name = data.Name;
            Password = data.Password;
            Group = data.Group;
            Attributes = data.Attributes;
            Disabled = data.Disabled;
            Comment = data.Comment;
        }
    }

}