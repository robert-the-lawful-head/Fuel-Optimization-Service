using FBOLinx.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using System.IO;
using FBOLinx.DB.Models;

namespace FBOLinx.Web.Services
{
    public class AssociationsService
    {
        private readonly IServiceProvider _services;
        private readonly FboLinxContext _context;
        public AssociationsService(FboLinxContext context, IServiceProvider services)
        {
            _context = context;
            _services = services;
        }

        public async Task<Association> CreateNewAssociation(string associationName)
        {
            Association association = new Association
            {
                AssociationName = associationName,
            };
            try
            {
                _context.Associations.Add(association);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }

            return association;
        }

        public async Task DeleteAssociation(int id)
        {
            var association = _context.Associations.Find(id);
            _context.Associations.Remove(association);

            await _context.SaveChangesAsync();
        }
    }
}
