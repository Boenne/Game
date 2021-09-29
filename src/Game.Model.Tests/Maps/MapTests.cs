using Game.Model.Maps;
using Game.Model.Resources;
using Shouldly;
using Xunit;

namespace Game.Model.Tests.Maps
{
    public class MapTests
    {
        [Fact]
        public void GetPosition_MapPointExists_ReturnsCoordinates()
        {
            var map = new Map(1);
            var resourceSite = new ResourceSite(typeof(Stone), 0);
            var resourceSiteCoordinates = new Coordinates(0, 0);
            map.SetLocation(resourceSiteCoordinates, resourceSite);

            var coordinates = map.GetPosition(resourceSite.Id);

            coordinates.ShouldBe(resourceSiteCoordinates);
        }

        [Fact]
        public void SetLocation_LocationIsAvailable_SetsLocation()
        {
            var map = new Map(1);
            var resourceSite = new ResourceSite(typeof(Stone), 0);
            var resourceSiteCoordinates = new Coordinates(0, 0);

            var wasSet = map.SetLocation(resourceSiteCoordinates, resourceSite);

            wasSet.ShouldBeTrue();
            map.GetPosition(resourceSite.Id).ShouldBe(resourceSiteCoordinates);
        }

        [Fact]
        public void SetLocation_LocationIsNotAvailable_ReturnsFalse()
        {
            var map = new Map(1);
            var resourceSite = new ResourceSite(typeof(Stone), 0);
            var resourceSite2 = new ResourceSite(typeof(Food), 0);
            var resourceSiteCoordinates = new Coordinates(0, 0);
            map.SetLocation(resourceSiteCoordinates, resourceSite);

            var wasSet = map.SetLocation(resourceSiteCoordinates, resourceSite2);

            wasSet.ShouldBeFalse();
        }

        [Fact]
        public void IsLocationAvailable_Available_ReturnsTrue()
        {
            var map = new Map(1);
            var resourceSiteCoordinates = new Coordinates(0, 0);

            var isLocationAvailable = map.IsLocationAvailable(resourceSiteCoordinates);

            isLocationAvailable.ShouldBeTrue();
        }

        [Fact]
        public void IsLocationAvailable_NotAvailable_ReturnsFalse()
        {
            var map = new Map(1);
            var resourceSite = new ResourceSite(typeof(Stone), 0);
            var resourceSiteCoordinates = new Coordinates(0, 0);
            map.SetLocation(resourceSiteCoordinates, resourceSite);

            var isLocationAvailable = map.IsLocationAvailable(resourceSiteCoordinates);

            isLocationAvailable.ShouldBeFalse();
        }

        [Fact]
        public void RemoveLocation_LocationIsUsed_ReturnsTrue()
        {
            var map = new Map(1);
            var resourceSite = new ResourceSite(typeof(Stone), 0);
            var resourceSiteCoordinates = new Coordinates(0, 0);
            map.SetLocation(resourceSiteCoordinates, resourceSite);

            var locationRemoved = map.RemoveLocation(resourceSiteCoordinates);

            locationRemoved.ShouldBeTrue();
        }

        [Fact]
        public void RemoveLocation_LocationIsNotUsed_ReturnsFalse()
        {
            var map = new Map(1);
            var resourceSiteCoordinates = new Coordinates(0, 0);

            var locationRemoved = map.RemoveLocation(resourceSiteCoordinates);

            locationRemoved.ShouldBeFalse();
        }
    }
}