﻿using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using RapidCore.Reflection;
using Skarp.HubSpotClient.Contact.Dto;
using Skarp.HubSpotClient.Core;
using Xunit;

namespace Skarp.HubSpotClient.UnitTest.Core
{
    public class ReflectionExtensionTest
    {


        [Fact]
        public void ReflectionExtension_resolves_prop_names_correctly()
        {
            var dto = new ClassWithDataMembers();
            var dtoProps = dto.GetType().GetProperties();

            var propNoCustomName = dtoProps.Single(p => p.Name == "MemberNoCustomName");
            Assert.Equal("MemberNoCustomName", propNoCustomName.GetPropSerializedName());
            var propWithCustomName = dtoProps.Single(p => p.Name == "MemberWithCustomName");
            Assert.Equal("CallMeThis", propWithCustomName.GetPropSerializedName());

        }

        [Fact]
        public void ReflectionExtension_can_find_methods_from_base_types_and_interfaces()
        {
            var dto = new ContactListHubSpotEntity<ContactHubSpotEntity>();

            // find the Add(T) method on the List entity!
            var listProp = dto.GetType().GetProperties().Single(p => p.Name == "Contacts");
            var method = listProp.PropertyType.FindMethodRecursively("Add", new[] { typeof(ContactHubSpotEntity) });

            Assert.NotNull(method);
        }

        [Fact]
        public void ReflectionExtension_can_determine_if_prop_has_ignore_data_member()
        {
            var dto = new ClassWithDataMembers();
            var propWithIgnore = dto.GetType().GetProperties().Single(p => p.Name == "IgnoreMePlease");
            var hasAttr = propWithIgnore.HasIgnoreDataMemberAttribute();
            Assert.True(hasAttr);

            var propNoIgnore = dto.GetType().GetProperties().Single(p => p.Name == "MemberWithCustomName");
            Assert.False(propNoIgnore.HasIgnoreDataMemberAttribute());
        }

        [DataContract]
        private class ClassWithDataMembers
        {

            [DataMember()]
            public string MemberNoCustomName { get; set; }

            [DataMember(Name = "CallMeThis")]
            public string MemberWithCustomName { get; set; }

            [IgnoreDataMember]
            public string IgnoreMePlease { get; set; }
        }
    }
}
