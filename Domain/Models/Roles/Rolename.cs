using System.ComponentModel.DataAnnotations;

namespace Domain{
    public enum Rolename{
        [Display(Name = "Developer")]
        Developer,

        [Display(Name = "Administrator")]
        Administrator,

        [Display(Name = "PanelMember")]
        PanelMember,

        [Display(Name = "Researcher")]
        Researcher,

        [Display(Name = "Usability_Expert")]
        UsabilityExpert,

        [Display(Name = "Accessibility_Expert")]
        AccessibilityExpert,

        [Display(Name = "Data_Analyst")]
        DataAnalyst,

        [Display(Name = "Company")]
        Company
        
    }
}