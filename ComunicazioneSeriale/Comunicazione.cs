using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComunicazioneSeriale
{
    class Comunicazione
    {
        public bool is_selected_port(int port_1_index,int port_2_index)
        {
            bool res = false;

            if (port_1_index >= 0 && port_2_index >= 0)
            {
                res = true;
            }
            
            return res;
        }
    }
}
