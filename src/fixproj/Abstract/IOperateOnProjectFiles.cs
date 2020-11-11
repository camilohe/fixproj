﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace fixproj.Abstract
{
    /// <summary>
    /// Provides operations on project files.
    /// </summary>
    interface IOperateOnProjectFiles
    {
        /// <summary>
        /// Represents a final version of project file.
        /// </summary>
        XDocument ModifiedDocument { get; }

        /// <summary>
        /// Represents a logger for changes are created.
        /// </summary>
        IList<string> Changes { get; }

        /// <summary>
        /// Fix project nodes.
        /// </summary>
        /// <returns></returns>
        List<ItemGroupEntity> FixContent();

        /// <summary>
        /// Deletes duplicate nodes.
        /// </summary>
        /// <param name="entity"></param>
        void DeleteDuplicates(ItemGroupEntity entity);

        /// <summary>
        /// Deletes references to non existent files.
        /// </summary>
        /// <param name="entity"></param>
        void DeleteReferencesToNonExistentFiles(ItemGroupEntity entity);

        /// <summary>
        /// Creates and sorts a final version of document.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sort"></param>
        void MergeAndSortItemGroups(ItemGroupEntity entity, bool sort);

        /// <summary>
        /// Writes detailed logging.
        /// </summary>
        void Verbose() => Changes.ForEach(x => Console.WriteLine(x));

        /// <summary>
        /// Sorts property nodes.
        /// </summary>
        void SortPropertyGroups()
        {
            ModifiedDocument.Root.ElementsByLocalName(Constants.PropertyGroupNode).ForEach(e => Sort(e));

            void Sort(XElement element, bool sortAttributes = true)
            {
                if (element == null)
                    throw new ArgumentNullException(nameof(element));

                if (sortAttributes)
                {
                    var atts = element.Attributes().OrderBy(a => a.ToString()).ToList();
                    atts.RemoveAll(x => true);
                    atts.ForEach(element.Add);
                }

                var sorted = element.Elements().OrderBy(e => e.Name.ToString()).ToList();
                if (!element.HasElements)
                    return;

                element.RemoveNodes();
                sorted.ForEach(c => Sort(c));
                sorted.ForEach(element.Add);
            }
        }
    }
}
