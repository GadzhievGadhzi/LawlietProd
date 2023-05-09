using Lawliet.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Lawliet.Controllers;

public class Task : IDataModel {
    public string Id { get; set; }
    public string? Titile { get; set; }
    public string? Description { get; set; }
    public string? Group { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
}