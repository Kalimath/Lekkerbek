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
        public Beoordeling(string titel, string commentaar, ScoreLijst scoreLijst, int klantId)
        {
            KlantId = klantId;
            Titel = titel;
            Commentaar = commentaar;
            ScoreLijst = scoreLijst;
            DefineTotalScore();
        }
        public Beoordeling(string titel, string commentaar, double hygieneScore, double serviceScore, double etenEnDrinkenScore, double prijsKwaliteitScore, int klantId)
        {
            KlantId = klantId;
            Titel = titel;
            Commentaar = commentaar;
            ScoreLijst = new ScoreLijst()
            {
                HygieneScore = hygieneScore,
                ServiceScore = serviceScore,
                EtenEnDrinkenScore = etenEnDrinkenScore,
                PrijsKwaliteitScore = prijsKwaliteitScore
            };
            DefineTotalScore();
        }

        public Beoordeling(string titel, string commentaar, double hygieneScore, double serviceScore, double etenEnDrinkenScore, double prijsKwaliteitScore)
        {
            Titel = titel;
            Commentaar = commentaar;
            ScoreLijst = new ScoreLijst()
            {
                HygieneScore = hygieneScore,
                ServiceScore = serviceScore,
                EtenEnDrinkenScore = etenEnDrinkenScore,
                PrijsKwaliteitScore = prijsKwaliteitScore
            };
            DefineTotalScore();
        }

        public Beoordeling(int id, string titel, string commentaar, ScoreLijst scoreLijst, int klantId)
        {
            Id = id;
            Titel = titel;
            Commentaar = commentaar;
            ScoreLijst = scoreLijst;
            KlantId = klantId;
        }

        public int Id { get; set; }

        [Required]
        [NotNull]
        public string Titel { get; set; }="/";
        [Required]
        [NotNull]
        [DisplayName("Scores")]
        public ScoreLijst ScoreLijst
        {
            get => _scoreLijst;
            set { _scoreLijst = value; }
        }

        [DisplayName("Algemene score")]
        public double TotaalScore
        {
            get => DefineTotalScore();
            set => _totaalScore = DefineTotalScore();
        }

        [DisplayName("Toelichting")]
        [Required]
        [NotNull]
        public string Commentaar { get; set; }
        public virtual int KlantId { get; set; }

        public double DefineTotalScore()
        {
            _totaalScore = (ScoreLijst.HygieneScore + ScoreLijst.ServiceScore + ScoreLijst.EtenEnDrinkenScore +
                            ScoreLijst.PrijsKwaliteitScore) / 4;
            return _totaalScore;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
