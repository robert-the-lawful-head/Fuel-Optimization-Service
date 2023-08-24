using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.Requests.FBO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Groups
{
    public interface IGroupFboService
    {
        Task<Group> CreateNewGroup(string groupName);
        Task<FbosDto> CreateNewFbo(SingleFboRequest request, AccountTypes accountType = AccountTypes.RevFbo);
        Task DeleteFbo(int fboId);
        Task DeleteFbos(List<int> fboIds);
    }
    public class GroupFboService : IGroupFboService
    {
        private FboLinxContext _context;
        private IServiceScopeFactory _serviceScopeFactory;
        private FuelerLinxContext _fuelerLinxContext;
        private FuelerLinxApiService _fuelerLinxApiService;
        private readonly IFboService _fboService;
        private readonly IFboAirportsService _fboAirportsService;

        public GroupFboService(FboLinxContext context, IServiceScopeFactory serviceScopeFactory, FuelerLinxContext fuelerLinxContext, FuelerLinxApiService fuelerLinxApiService, IFboService fboService, IFboAirportsService fboAirportsService)
        {
            _fuelerLinxContext = fuelerLinxContext;
            _serviceScopeFactory = serviceScopeFactory;
            _context = context;
            _fuelerLinxApiService = fuelerLinxApiService;
            _fboService = fboService;
            _fboAirportsService = fboAirportsService;
        }

        #region Public Methods

        public async Task<Group> CreateNewGroup(string groupName)
        {
            Group group = new Group
            {
                GroupName = groupName,
                Active = true
            };
            try
            {
                _context.Group.Add(group);
                await _context.SaveChangesAsync();

                if (group.Oid != 0)
                {
                    try
                    {
                        await _fuelerLinxContext.Database.ExecuteSqlRawAsync("exec up_Insert_FBOlinxGroupIntofuelerList @GroupName='" + group.GroupName.Replace("'","''") + "', @GroupID=" + group.Oid + "");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("SP error: " + ex.Message);
                    }

                    try
                    {
                        var task = Task.Run(async () =>
                        {
                            using (var scope = _serviceScopeFactory.CreateScope())
                            {
                                var groupCustomersService = scope.ServiceProvider.GetRequiredService<IGroupCustomersService>();
                                await groupCustomersService.StartAircraftTransfer(group.Oid);
                            }

                        });
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Customer Aircraft add error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Global error: " + ex.Message);
            }

            return group;
        }

        public async Task<FbosDto> CreateNewFbo(SingleFboRequest request, AccountTypes accountType = AccountTypes.RevFbo)
        {
            if (request.GroupId.GetValueOrDefault() == 0)
            {
                var group = await CreateNewGroup(request.Group);
                if (group == null || group.Oid == 0)
                    throw new Exception("There was an issue creating a new group.");
                request.GroupId = group.Oid;
            }

            FbosDto fbo = new FbosDto
            {
                Fbo = request.Fbo.Trim(),
                GroupId = request.GroupId.GetValueOrDefault(),
                AcukwikFBOHandlerId = request.AcukwikFboHandlerId,
                Active = true,
                DateActivated = DateTime.Now,
                AccountType = accountType
            };

            await _fboService.AddAsync(fbo);

            //_context.Fbos.Add(fbo);
            //await _context.SaveChangesAsync();

            FboAirportsDTO fboairport = new FboAirportsDTO
            {
                Icao = request.Icao,
                Iata = request.Iata,
                Fboid = fbo.Oid
            };
            await _fboAirportsService.AddAsync(fboairport);
            //_context.Fboairports.Add(fboairport);
            //await _context.SaveChangesAsync();

            return fbo;
        }

        public async Task DeleteFbo(int fboId)
        {
            await _context.Database.ExecuteSqlRawAsync("exec up_Delete_Fbo @OID = " + fboId);
        }
        public async Task DeleteGroup(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("exec up_Delete_Group @OID = " + id);

            List<int> groupfboIds = await _context.Fbos.Where(f => f.GroupId.Equals(id)).Select(f => f.Oid).ToListAsync();
            await DeleteFbos(groupfboIds);

            var group = _context.Group.Find(id);
            _context.Group.Remove(group);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteFbos(List<int> fboIds)
        {
            foreach (int fboId in fboIds)
            {
                await DeleteFbo(fboId);
            }
        }

        #endregion
    }
}