using FashionGo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionGo.Models
{
    public class SoSanh
    {
        public Product[] ListSp = new Product[2];
        public Product sp1 { get { return ListSp[0]; } }
        public Product sp2 { get { return ListSp[1]; } }
        public void Add(Product sp1, Product sp2)
        {
            ListSp = new Product[2];
            ListSp[0]=(sp1);
            ListSp[1]=(sp2);
        }
        public bool Add(Product sp, int index)
        {
            try
            {
                ListSp[index] = (sp);return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public void AddFirst(Product sp)
        {
            ListSp[0] = (sp);
        }
        public void AddLast(Product sp)
        {
            ListSp[1] = (sp);
        }
    }
}