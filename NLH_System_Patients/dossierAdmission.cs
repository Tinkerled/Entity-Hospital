//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NLH_System_Patients
{
    using System;
    using System.Collections.Generic;
    
    public partial class dossierAdmission
    {
        public int num { get; set; }
        public Nullable<int> docteur { get; set; }
        public Nullable<int> patient { get; set; }
        public Nullable<int> lit { get; set; }
        public string location { get; set; }
        public string traitement { get; set; }
        public string etat { get; set; }
        public Nullable<System.DateTime> dateAdmission { get; set; }
        public Nullable<System.DateTime> dateSortie { get; set; }
        public string noAssurance { get; set; }
    
        public virtual docteur docteur1 { get; set; }
        public virtual lit lit1 { get; set; }
        public virtual location location1 { get; set; }
        public virtual patient patient1 { get; set; }
        public virtual traitement traitement1 { get; set; }
    }
}
