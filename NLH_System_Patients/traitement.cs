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
    
    public partial class traitement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public traitement()
        {
            this.dossierAdmissions = new HashSet<dossierAdmission>();
        }
    
        public string num { get; set; }
        public string nom { get; set; }
        public string departement { get; set; }
    
        public virtual departement departement1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dossierAdmission> dossierAdmissions { get; set; }
    }
}
