using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FortiOS.Types
{
    /************************************************************************************************************
     * LISTA WSZYSTKICH WYKRYTYCH URZĄDZEŃ  
     ************************************************************************************************************/

    public class FortiRegisterDevice
    {
        [JsonPropertyName("is_online")]
        public bool IsOnline { get; set; }

        [JsonPropertyName("ipv4_address")]
        public String IP { get; set; }

        [JsonPropertyName("hostname")]
        public String Hostname { get; set; }
      
        [JsonPropertyName("mac")]
        public String MAC{ get; set; }

        [JsonPropertyName("detected_interface")]
        public String Interface { get; set; }

        [JsonPropertyName("hardware_vendor")]
        public String HardwareVendor { get; set; }

        [JsonPropertyName("vdom")]
        public String VDOM { get; set; }


        [JsonPropertyName("active_start_time")]
        public int active_start_time { get; set; }


        [JsonPropertyName("dhcp_lease_status")]
        public String dhcp_lease_status { get; set; }

        [JsonPropertyName("dhcp_lease_reserved")]
        public bool dhcp_lease_reserved { get; set; }

        [JsonPropertyName("dhcp_server_id")]
        public int dhcp_server_id { get; set; }


        [JsonPropertyName("unjoined_forticlient_endpoint")]
        public bool unjoined_forticlient_endpoint { get; set; }

        [JsonPropertyName("is_fortiguard_src")]
        public bool is_fortiguard_src { get; set; }

        [JsonPropertyName("master_mac")]
        public String master_mac { get; set; }

        [JsonPropertyName("is_master_device")]
        public bool is_master_device { get; set; }

        [JsonPropertyName("is_detected_interface_role_wan")]
        public bool Is_Detected_Interface_Tole_Wan { get; set; }

        [JsonPropertyName("detected_interface_fortitelemetry")]
        public bool detected_interface_fortitelemetry { get; set; }

        [JsonPropertyName("online_interfaces")]
        public List<String> Online_Interfaces { get; set; }

        public FortiRegisterDevice()
        {
            this.IP = "";
            this.MAC = "";
            this.Interface = "";
            this.active_start_time = 0;
            this.IsOnline = false;
            this.HardwareVendor = "";
            Online_Interfaces = new List<String>();
            this.dhcp_lease_status = "";
            this.Hostname = "";
            this.master_mac = "";
            this.detected_interface_fortitelemetry = false;
            this.dhcp_lease_status = "";
            this.is_master_device = false;
            this.dhcp_server_id = 0;
            this.unjoined_forticlient_endpoint = false;
        }
    }

    public class FortiResultNetworkDevices
    {
        [JsonPropertyName("http_method")]
        public String Method;

        [JsonPropertyName("results")]
        public List<FortiRegisterDevice> clients { get; set; }

        [JsonPropertyName("vdom")]
        public String vdom { get; set; }

        [JsonPropertyName("query_type")]
        public String query_type { get; set; }

        [JsonPropertyName("count")]
        public int count { get; set; }

        [JsonPropertyName("total")]
        public int total { get; set; }

        [JsonPropertyName("start")]
        public int start { get; set; }

        [JsonPropertyName("number")]
        public int number { get; set; }

        [JsonPropertyName("status")]
        public String status { get; set; }

        [JsonPropertyName("serial")]
        public String serial { get; set; }

        [JsonPropertyName("version")]
        public String version { get; set; }

        [JsonPropertyName("build")]
        public int build { get; set; }



        public FortiResultNetworkDevices()
        {
            this.Method = "";
            this.clients = new List<FortiRegisterDevice>();
        }
    }



    /************************************************************************************************************
    * LISTA WSZYSTKICH WPISÓW DHCP
    ************************************************************************************************************/



    public class FortiDHCPLease
    {
        [JsonPropertyName("ip")]
        public String IP { get; set; }

        [JsonPropertyName("hostname")]
        public String HostName { get; set; }

        [JsonPropertyName("mac")]
        public String MAC { get; set; }

        [JsonPropertyName("interface")]
        public String Interface { get; set; }

        [JsonPropertyName("reserved")]
        public bool Reserved { get; set; }


        [JsonPropertyName("status")]
        public String Status { get; set; }


        public FortiDHCPLease()
        {
            this.IP = "";
            this.MAC = "";
            this.Status = "";
            this.HostName = "";
            this.Interface = "";
            this.Reserved= false;
            this.Status = "";
        }
    }



    public class FortiResultDHCPLease
    {
        [JsonPropertyName("http_method")]
        public String Method { get; set; }

        [JsonPropertyName("results")]
        public List<FortiDHCPLease> clients { get; set; }

        [JsonPropertyName("vdom")]
        public String vdom { get; set; }

        [JsonPropertyName("query_type")]
        public String query_type { get; set; }

        [JsonPropertyName("count")]
        public int count { get; set; }

        [JsonPropertyName("total")]
        public int total { get; set; }

        [JsonPropertyName("start")]
        public int start { get; set; }

        [JsonPropertyName("number")]
        public int number { get; set; }

        [JsonPropertyName("status")]
        public String status { get; set; }

        [JsonPropertyName("serial")]
        public String serial { get; set; }

        [JsonPropertyName("version")]
        public String version { get; set; }

        [JsonPropertyName("build")]
        public int build { get; set; }


        public FortiResultDHCPLease()
        {
            this.Method = "";
            this.clients = new List<FortiDHCPLease>();
        }
    }


    /************************************************************************************************************
    * LISTA WSZYSTKICH WPISÓW VPN USERS
    ************************************************************************************************************/

    public class FortiVPNUser
    {
        [JsonPropertyName("name")]
        public String Name { get; set; }
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("status")]
        public String Status { get; set; }

        [JsonPropertyName("ldap-server")]
        public String ServerLDAP { get; set; }

        [JsonPropertyName("radius-server")]
        public String ServerRADIUS { get; set; }

        [JsonPropertyName("two-factor")]
        public String TwoFactor { get; set; }

        [JsonPropertyName("email-to")]
        public String Email { get; set; }

        [JsonPropertyName("passwd-time")]
        public String PasswordTime { get; set; }

        public FortiVPNUser()
        {
            Name = "";
            ID = 0;
            Status = "";
            ServerLDAP = "";
            ServerRADIUS = "";
            TwoFactor = "";
            Email = "";
            PasswordTime = "";
        }
    }

    public class FortiResultVPNUsers
    {
        [JsonPropertyName("http_method")]
        public String Method { get; set; }

        [JsonPropertyName("results")]
        public List<FortiVPNUser> users { get; set; }

        [JsonPropertyName("vdom")]
        public String vdom { get; set; }

        [JsonPropertyName("query_type")]
        public String query_type { get; set; }

        [JsonPropertyName("count")]
        public int count { get; set; }

        [JsonPropertyName("total")]
        public int total { get; set; }

        [JsonPropertyName("start")]
        public int start { get; set; }

        [JsonPropertyName("number")]
        public int number { get; set; }

        [JsonPropertyName("status")]
        public String status { get; set; }

        [JsonPropertyName("serial")]
        public String serial { get; set; }

        [JsonPropertyName("version")]
        public String version { get; set; }

        [JsonPropertyName("build")]
        public int build { get; set; }


        public FortiResultVPNUsers()
        {
            this.Method = "";
            this.users = new List<FortiVPNUser>();
        }
    }
    /************************************************************************************************************
     * LISTA DHCP SERVER LIST
     ************************************************************************************************************/

    public class FortiIPRange
    {
        [JsonPropertyName("start-ip")]
        public String StartIP { get; set; }
        [JsonPropertyName("end-ip")]
        public String EndIP { get; set; }

        public FortiIPRange()
        {
            StartIP = "";
            EndIP = "";
        }
    }

    public class FortiReservedAddress
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("ip")]
        public String AddressIP { get; set; }
        [JsonPropertyName("mac")]
        public String AddressMAC { get; set; }
        [JsonPropertyName("description")]
        public String Comment { get; set; }

        public FortiReservedAddress()
        {
            ID = 0;
            AddressIP = "";
            AddressMAC = "";
            Comment = "";
        }
    }

    public class FortiDHCPServer
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("status")]
        public String Status { get; set; }
        [JsonPropertyName("lease-time")]
        public int LeaseTime { get; set; }

        [JsonPropertyName("dns-service")]
        public String DNSService { get; set; }
        [JsonPropertyName("dns-server1")]
        public String DNSServer1 { get; set; }
        [JsonPropertyName("dns-server2")]
        public String DNSServer2 { get; set; }
        [JsonPropertyName("dns-server3")]
        public String DNSServer3 { get; set; }

        [JsonPropertyName("ntp-server1")]
        public String NTPServer1 { get; set; }
        [JsonPropertyName("ntp-server2")]
        public String NTPServer2 { get; set; }
        [JsonPropertyName("ntp-server3")]
        public String NTPServer3 { get; set; }
        [JsonPropertyName("domain")]
        public String Domain { get; set; }
        [JsonPropertyName("default-gateway")]
        public String Gateway { get; set; }
        [JsonPropertyName("netmask")]
        public String Netmask { get; set; }
        [JsonPropertyName("interface")]
        public String InterfaceeName { get; set; }

        [JsonPropertyName("ip-range")]
        public List<FortiIPRange> IPRange { get; set; }
        [JsonPropertyName("reserved-address")]
        public List<FortiReservedAddress> ReservedAddress { get; set; }

        public FortiDHCPServer()
        {
            ReservedAddress = new List<FortiReservedAddress>() { };
            IPRange = new List<FortiIPRange>();
            InterfaceeName = "";
            Gateway = "";
            Netmask = "";
            Domain = "";
            DNSService = "";
            NTPServer1 = "";
            NTPServer2 = "";
            NTPServer3 = "";
            DNSServer1 = "";
            DNSServer2 = "";
            DNSServer3 = "";

            LeaseTime = 0;
            Status = "";
            ID = 0;
        }
    }

    public class FortiResultDHCPServers
    {
        [JsonPropertyName("http_method")]
        public String Method { get; set; }

        [JsonPropertyName("results")]
        public List<FortiDHCPServer> servers { get; set; }

        [JsonPropertyName("vdom")]
        public String vdom { get; set; }

        [JsonPropertyName("query_type")]
        public String query_type { get; set; }

        [JsonPropertyName("count")]
        public int count { get; set; }

        [JsonPropertyName("total")]
        public int total { get; set; }

        [JsonPropertyName("start")]
        public int start { get; set; }

        [JsonPropertyName("number")]
        public int number { get; set; }

        [JsonPropertyName("status")]
        public String status { get; set; }

        [JsonPropertyName("serial")]
        public String serial { get; set; }

        [JsonPropertyName("version")]
        public String version { get; set; }

        [JsonPropertyName("build")]
        public int build { get; set; }


        public FortiResultDHCPServers()
        {
            this.Method = "";
            this.servers = new List<FortiDHCPServer>();
        }
    }

}
