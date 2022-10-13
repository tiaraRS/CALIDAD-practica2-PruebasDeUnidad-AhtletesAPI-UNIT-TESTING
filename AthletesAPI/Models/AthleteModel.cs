using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AthletesRestAPI.Models
{
    //[JsonConverter(typeof(StringEnumConverter))]
    public enum Gender { 
        F, M
    }
    
    public class AthleteModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } 
        public string Nationality { get; set; }
        public bool? IsActive { get; set; }
        public int? NumberOfCompetitions { get; set; }

        [Required]
        public int DisciplineId { get; set; }

        [Required]
        public Gender? Gender { get; set; }
        
        public Decimal PersonalBest { get; set; }

        public decimal? SeasonBest { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? BirthDate { get; set; }
        public int? Points { get; set; }

        //images
        public string ImagePath { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is AthleteModel))
            {
                return false;
            }

            AthleteModel other = (AthleteModel)obj;
            return this.Id == other.Id &&
                this.Name == other.Name &&
                this.Nationality == other.Nationality &&
                this.IsActive == other.IsActive &&
                this.NumberOfCompetitions == other.NumberOfCompetitions &&
                this.DisciplineId == other.DisciplineId &&
                this.Gender == other.Gender &&
                this.PersonalBest == other.PersonalBest &&
                this.SeasonBest == other.SeasonBest &&
                this.Points == other.Points &&
                this.ImagePath == other.ImagePath;
        }

    }
}
