using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace mdx
{
    internal class mdxCache
    {
        private Dictionary<string, List<mdxItem>> dicCachedItems = new Dictionary<string, List<mdxItem>>();


        public mdxWriteResult Write(mdxItem writeItem)
        {
            var validator = mdxValidation.mdxValidator.ValidatorForItem(writeItem.itemHeader.Type);

            if (validator != null) {
                if (validator.validate(writeItem.itemContent.Content))
                {
                    if (dicCachedItems.ContainsKey(writeItem.Identifier))
                    {
                        writeItem.itemHeader.Version = dicCachedItems[writeItem.Identifier].Count + 1;
                        dicCachedItems[writeItem.Identifier].Add(writeItem);
                    }
                    else
                    {
                        writeItem.itemHeader.Version = 1;
                        dicCachedItems.Add(writeItem.Identifier, new List<mdxItem> { writeItem });
                    }

                    return new mdxWriteResult("OK", "Written at " + DateTime.Now.ToString(), writeItem.itemHeader); ;
                }
                else { throw new Exception("Validation failed"); }
            }
            else 
            { 
                throw new Exception("Failed to find a suitable validator"); 
            }
        }

        public mdxItem Read(string identifier)
        {
            
            if (dicCachedItems.ContainsKey(identifier))
            {
                var items = dicCachedItems[identifier];
                return items.Last();
            }
            else
            {
                return null;
            }
        }
    }
}
