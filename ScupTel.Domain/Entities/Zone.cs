using System.Collections.Generic;

namespace ScupTel.Domain
{
    public class Zone
    {
        public Zone()
        {

        }

        public Zone(string name, int code)
        {
            Name = name;
            Code = code;
        }
        public int Id { get; private set; }

        public string Name { get; set; }

        public int Code { get; set; }
 

    }
}