using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.AcukwikAirport
{
    public class AcukwikAirportSpecification : Specification<Models.AcukwikAirport>
    {
        public AcukwikAirportSpecification() : base(x => true)
        {
        }

        public AcukwikAirportSpecification(IList<string> airportICAOs) : base(x => airportICAOs.Contains(x.Icao))
        {
        }
    }
}