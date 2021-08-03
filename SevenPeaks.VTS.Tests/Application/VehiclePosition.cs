using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SevenPeaks.VTS.Application.Vehicle.Commands.AddVehicle;
using SevenPeaks.VTS.Application.VehiclePosition.Commands.AddVehiclePosition;
using SevenPeaks.VTS.Application.VehiclePosition.Queries.GetVehiclePositions;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Infrastructure.Interfaces;
using SevenPeaks.VTS.Persistence;
using Xunit;

namespace SevenPeaks.VTS.Tests.Application
{
    public class VehiclePosition
    {
        private readonly DbContextOptions<DatabaseService> _dbContextOptions;
        private readonly IDatabaseService _context;
        private readonly IUriService _uriService;
        private const string Url = "http://120.0.0.1/Timesheet/Index?pageNumber=1&pageSize=10";
        private readonly string _userId = Guid.NewGuid().ToString();
        private readonly DateTime _workDay = DateTime.Now;
        public VehiclePosition()
        {
            string databaseName = Guid.NewGuid().ToString();

            _dbContextOptions = new DbContextOptionsBuilder<DatabaseService>()
                .UseInMemoryDatabase(databaseName)
                .Options;


            var mock = new Mock<IAuthenticatedUser>();
            mock.Setup(m => m.UserId).Returns(_userId);
            _context = new DatabaseService(_dbContextOptions, mock.Object);

            var mockUriService = new Mock<IUriService>();
            mockUriService.Setup(service => service.GetPageUri(It.IsAny<PaginationFilter>(), It.IsAny<string>()))
                .Returns(new Uri(Url));
            _uriService = mockUriService.Object;

        }
        
       
        [Fact]
        public async Task AddVehiclePositionInSuccessTest()
        {
            var loggerMock = new Mock<ILogger<AddVehicleCommand>>();
            var loggerMockPosition = new Mock<ILogger<AddVehiclePositionCommand>>();
             
            IAddVehicleCommand add = new AddVehicleCommand(_context, loggerMock.Object);
            IAddVehiclePositionCommand addPosition = new AddVehiclePositionCommand(_context, loggerMockPosition.Object);
            var expected1 = await add.Execute(new AddVehicleModel(){});
            var expected2 = await add.Execute(new AddVehicleModel(){});

            var expected = await addPosition.Execute(new AddVehiclePositionModel(){ DeviceId = "dd", VehicleId = 1});

            Assert.NotNull(expected2);
            Assert.Equal(200, expected.ResponseCode);
            
        }

        [Fact]
        public async Task AddVehiclePositionInFailureTest()
        {
            var loggerMock = new Mock<ILogger<AddVehicleCommand>>();
            var loggerMockPosition = new Mock<ILogger<AddVehiclePositionCommand>>();
             
           
            IAddVehiclePositionCommand addPosition = new AddVehiclePositionCommand(_context, loggerMockPosition.Object);
           
            var expected = await addPosition.Execute(new AddVehiclePositionModel(){ DeviceId = "ddww", VehicleId = 1});

            Assert.Equal(404, expected.ResponseCode);
            Assert.Equal("Vehicle not found", expected.Message);
        }
        
        [Fact]
        public async Task GetVehiclePositionsInFailureTest()
        {
            var loggerMock = new Mock<ILogger<AddVehicleCommand>>();
            var loggerMockPosition = new Mock<ILogger<AddVehiclePositionCommand>>();
             
            IAddVehicleCommand add = new AddVehicleCommand(_context, loggerMock.Object);
            IAddVehiclePositionCommand addPosition = new AddVehiclePositionCommand(_context, loggerMockPosition.Object);
            IGetVehiclePositionsQuery getVehiclePositions = new GetVehiclePositionsQuery(_context, _uriService);
            var expected0 = await add.Execute(new AddVehicleModel(){ PlateNumber = "AAA"});
             
            var expected7 = await addPosition.Execute(new AddVehiclePositionModel(){ DeviceId = expected0.Result, VehicleId = 1});
            var expected1 = await addPosition.Execute(new AddVehiclePositionModel(){ DeviceId = expected0.Result, VehicleId = 1});
            var expected2 = await addPosition.Execute(new AddVehiclePositionModel(){ DeviceId = expected0.Result, VehicleId = 1});
            var expected3 = await addPosition.Execute(new AddVehiclePositionModel(){ DeviceId = expected0.Result, VehicleId = 1});
            var expected4 = await addPosition.Execute(new AddVehiclePositionModel(){ DeviceId = expected0.Result, VehicleId = 1});
            var expected = await getVehiclePositions.Execute(new VehiclePositionsQuery()
            {
                PlateNumber = "AAA"
            });
            
            Assert.Equal(200, expected.ResponseCode);
            Assert.Equal(5, expected.Result.TotalNumberOfRecords);
        }
        
         
    }
}
 