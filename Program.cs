using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

namespace dotnetfhirconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello FHIR, creating a patient!");

            var pat = new Patient();

            var id = new Identifier();
            id.System = "http://hl7.org/fhir/sid/us-ssn";
            id.Value = "000-12-3456";
            pat.Identifier.Add(id);

            var name = new HumanName().WithGiven("Christopher").WithGiven("C.H.").AndFamily("Parks");
            name.Prefix = new string[] { "Mr." };
            name.Use = HumanName.NameUse.Official;

            var nickname = new HumanName();
            nickname.Use = HumanName.NameUse.Nickname;
            nickname.GivenElement.Add(new FhirString("Chris"));

            pat.Name.Add(name);
            pat.Name.Add(nickname);

            pat.Gender = AdministrativeGender.Male;

            pat.BirthDate = "1983-04-23";

            var birthplace = new Extension();
            birthplace.Url = "http://hl7.org/fhir/StructureDefinition/birthPlace";
            birthplace.Value = new Address() { City = "Seattle" };
            pat.Extension.Add(birthplace);

            var birthtime = new Extension("http://hl7.org/fhir/StructureDefinition/patient-birthTime",
                                           new FhirDateTime(1983, 4, 23, 7, 44));
            pat.BirthDateElement.Extension.Add(birthtime);

            var address = new Address()
            {
                Line = new string[] { "3300 Washtenaw Avenue, Suite 227" },
                City = "Ann Arbor",
                State = "MI",
                PostalCode = "48104",
                Country = "USA"
            };
            pat.Address.Add(address);

            var contact = new Patient.ContactComponent();
            contact.Name = new HumanName();
            contact.Name.Given = new string[] { "Susan" };
            contact.Name.Family = "Parks";
            contact.Gender = AdministrativeGender.Female;
            contact.Relationship.Add(new CodeableConcept("http://hl7.org/fhir/v2/0131", "N"));
            contact.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Phone, null, ""));
            pat.Contact.Add(contact);


            var serializer = new FhirXmlSerializer();
            var xmlText = serializer.SerializeToString(pat);

            Console.WriteLine(xmlText);


            var eld = new ElementDefinition();
            eld.ElementId = "Observation.value[x]:valueQuantity.code";
            eld.Path = "Observation.value[x].code";
            eld.Min = 1;

            eld.Fixed = new Code("s");

            xmlText = serializer.SerializeToString(eld);

            Console.WriteLine(xmlText);

 
        }
    }
}
