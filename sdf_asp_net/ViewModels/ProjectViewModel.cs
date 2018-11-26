using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sdf_asp_net.ViewModels
{
    public class ProjectViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ArrayList Member { get; set; }

        public ProjectViewModel()
        {
            Member = new ArrayList();
        }

        public void ConvertStringMemberToArrayListMember(string member)
        {
            string[] splitedMembers = member.Split(';');
            for(int i = 0; i < splitedMembers.Length; i++)
            {
                this.Member.Add(splitedMembers[i]);
            }
        }
    }
}