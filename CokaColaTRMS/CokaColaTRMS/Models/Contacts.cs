using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CokaColaTRMS.Models
{
   public class ContactModel
   {
       #region Properties

       public int ID { get; set; }
       public string Name { get; set; }
       public string TelephoneNumber { get; set; }

       #endregion


       #region Methods

       public static List<ContactModel> GetAllContacts()
       {
           try
           {
               using (CokeTRMSEntities context=new CokeTRMSEntities())
               {
                   return context.Contacts.Select(c => new ContactModel
                   {
                       ID = c.Id,
                       Name = c.Name,
                       TelephoneNumber = c.TelephoeNumber

                   }).ToList();
               }
           }
           catch (Exception ex)
           {
               return new List<ContactModel>();
           }
       }

       #endregion

   }

   public class ImportContactsModel
   {
       public string UniqueID { get; set; }
       public string Name { get; set; }
       public string TelephoeNumber { get; set; }
       public string Message { get; set; }
       public bool MessageSent { get; set; }

       public bool AddImportedContacts(List<ImportContactsModel> importedContacts)
       {
           try
           {
               using (CokeTRMSEntities context=new CokeTRMSEntities())
               {
                   foreach (ImportContactsModel contact in importedContacts)
                   {
                       context.Contacts.AddObject(new Contact
                                {
                                     Name = contact.Name,
                                     TelephoeNumber = contact.TelephoeNumber
                                }); 
                   }

                   context.SaveChanges();
                   return true;
               }
           }
           catch (Exception)
           {
               return false;
           }
       }
   }

}
