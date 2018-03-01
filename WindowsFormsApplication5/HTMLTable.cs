using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    class HTMLTable
    {
        private string m_table_str;
        private string m_thead_str;
        private string m_tbody_str;
        private string m_tfoot_str;

        private string TheadStr
        {
            get
            {
                return m_thead_str += "</thead>";
            }
        }

        private string TbodyStr
        {
            get
            {
                return m_tbody_str += "</tbody>";
            }
        }

        private string TfootStr
        {
            get
            {
                return m_tfoot_str += "</tfoot>";
            }
        }

        public string TableStr
        {
            get
            {
                return m_table_str + TheadStr + TbodyStr + TfootStr + "</table>";
            }
        }

        public HTMLTable(string p_table_name)
        {
            m_table_str = "<table name=\"" + p_table_name + "\" class=\"pure-table pure-table-bordered sortable\">";
            m_thead_str = "<thead>";
            m_tbody_str = "<tbody>";
            m_tfoot_str = "<tfoot>";
        }

        public void AddHeaderRow(List<string> p_header_row)
        {
            m_thead_str += "<tr>";
            foreach (var header in p_header_row)
            {
                m_thead_str += "<th>" + header + "</th>";
            }
            m_thead_str += "</tr>";
        }

        public void AddFooterRow(List<string> p_footer_row)
        {
            m_tfoot_str += "<tr>";
            foreach (var str in p_footer_row)
            {
                m_tfoot_str += "<td>" + str + "</td>";
            }
            m_tfoot_str += "</tr>";
        }

        public void AddRow(List<string> p_row)
        {
            m_tbody_str += "<tr>";
            foreach (var str in p_row)
            {
                m_tbody_str += "<td>" + str + "</td>";
            }
            m_tbody_str += "</tr>";
        }

        public void AddRows(List<List<string>> p_rows)
        {
            foreach (var row in p_rows)
            {
                AddRow(row);
            }
        }


    }
}
