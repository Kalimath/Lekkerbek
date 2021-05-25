using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace Lekkerbek.Web.Models
{
    public class Beoordeling : IDisposable
    {
        private ScoreLijst _scoreLijst;
        private double _totaalScore;

        public Beoordeling()
        {

        }
        public Beoordeling(string commentaar, ScoreLijst scoreLijst, int klantId)
        {
            KlantId = klantId;
            Commentaar = commentaar;
            ScoreLijst = scoreLijst;
            TotaalScore = (ScoreLijst.HygieneScore + ScoreLijst.ServiceScore + ScoreLijst.EtenEnDrinkenScore + ScoreLijst.PrijsKwaliteitScore) / 4;
        }
        public Beoordeling(string commentaar, double hygieneScore, double serviceScore, double etenEnDrinkenScore, double prijsKwaliteitScore, int klantId)
        {
            KlantId = klantId;
            Commentaar = commentaar;
            ScoreLijst = new ScoreLijst()
            {
                HygieneScore = hygieneScore,
                ServiceScore = serviceScore,
                EtenEnDrinkenScore = etenEnDrinkenScore,
                PrijsKwaliteitScore = prijsKwaliteitScore
            };
            TotaalScore = (ScoreLijst.HygieneScore + ScoreLijst.ServiceScore + ScoreLijst.EtenEnDrinkenScore + ScoreLijst.PrijsKwaliteitScore) / 4;
        }

        public Beoordeling(string commentaar, double hygieneScore, double serviceScore, double etenEnDrinkenScore, double prijsKwaliteitScore)
        {
            Commentaar = commentaar;
            ScoreLijst = new ScoreLijst()
            {
                HygieneScore = hygieneScore,
                ServiceScore = serviceScore,
                EtenEnDrinkenScore = etenEnDrinkenScore,
                PrijsKwaliteitScore = prijsKwaliteitScore
            };
            TotaalScore = (ScoreLijst.HygieneScore + ScoreLijst.ServiceScore + ScoreLijst.EtenEnDrinkenScore + ScoreLijst.PrijsKwaliteitScore) / 4;
        }


        public int Id { get; set; }

        [Required]
        [NotNull]
        [DisplayName("Scores")]
        public ScoreLijst ScoreLijst
        {
            get => _scoreLijst;
            set { _scoreLijst = value; }
        }

        [DisplayName("Totale score")]
        public double TotaalScore
        {
            get => (ScoreLijst.HygieneScore + ScoreLijst.ServiceScore + ScoreLijst.EtenEnDrinkenScore + ScoreLijst.PrijsKwaliteitScore) / 4;
            set => _totaalScore = value;
        }

        [DisplayName("Toelichting")]
        [Required]
        [NotNull]
        public string Commentaar { get; set; }
        public virtual int KlantId { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
