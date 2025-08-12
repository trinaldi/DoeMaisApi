using System.ComponentModel.DataAnnotations;

namespace DoeMais.Domain.Enums;

public enum Category
{
    [Display(Name = "Brinquedos")]
    Brinquedos = 1,
    [Display(Name = "Eletrônicos")]
    Eletronicos,
    [Display(Name = "Eletrodomésticos")]
    Eletrodomesticos,
    [Display(Name = "Equipamentos Médicos")]
    EquipamentosMedicos,
    [Display(Name = "Higiene Pessoal")]
    HigienePessoal,
    [Display(Name = "Livros")]
    Livros,
    [Display(Name = "Material Escolar")]
    MaterialEscolar,
    [Display(Name = "Móveis")]
    Móveis,
    [Display(Name = "Vestuário")]
    Vestuario,
    [Display(Name = "Outros")]
    Outros
    
}