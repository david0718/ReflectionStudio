using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AvalonDock;
using ReflectionStudio.Components.UserControls;
using ReflectionStudio.Core.Events;
using System.Resources;
using System.Reflection;
using System.IO;
using System.Xml;
using ReflectionStudio.Classes.Workspace;

namespace ReflectionStudio.Classes
{
	internal class DocumentFactory
	{
		#region ----------------SINGLETON----------------
		public static readonly DocumentFactory Instance = new DocumentFactory();

		/// <summary>
		/// Private constructor for singleton pattern
		/// </summary>
		private DocumentFactory()
		{
		}
		#endregion

		#region ---------------------PRIVATE---------------------

		private DockingManager _DockManager = null;

		#endregion

		#region ----------------PROPERTIES----------------


		public DocumentContent ActiveDocument
		{
			get { return _DockManager.ActiveDocument as DocumentContent; }
		}

		List<SupportedDocumentInfo> _SupportedDocuments = new List<SupportedDocumentInfo>();
		public List<SupportedDocumentInfo> SupportedDocuments
		{
			get { return _SupportedDocuments; }
		}

		#endregion

		#region ----------------METHODS----------------

		public void Initialize(DockingManager dockManager)
		{
			_DockManager = dockManager;

			InitSupportedDocuments();
		}

		public DocumentContent CreateDocument(SupportedDocumentInfo info, DocumentDataContext context = null)
		{
			return GetDocument(info, context, true);
		}


        public DocumentContent OpenDocument(SupportedDocumentInfo info, DocumentDataContext context )
		{
			return GetDocument(info, context, true);
		}

		//public void CloseDocument(SupportedDocumentInfo info, DocumentDataContext context)
		//{
		//    Tracer.Verbose("DocumentFactory:CloseDocument", "docName {0}", docName);

		//    try
		//    {
		//        IEnumerable<DocumentContent> docs = _DockManager.Documents.Where(d => d.Name == docName);

		//        if (docs.Count() == 0)
		//            return;
		//        else
		//        {
		//            foreach (DocumentContent doc in docs)
		//                doc.Close();
		//        }
		//    }
		//    catch (Exception err)
		//    {
		//        Tracer.Error("DocumentFactory.CloseDocument", err);
		//    }
		//    finally
		//    {
		//        Tracer.Verbose("DocumentFactory:CloseDocument", "END");
		//    }
		//}

		#endregion

		#region ---------------------EVENTS---------------------

		private void DocumentClosing(object sender, CancelEventArgs e)
		{
			Tracer.Verbose("DocumentFactory.DocumentClosing", ((DocumentContent)sender).Title + " closing");

			//manage dirty state ?
		}

		private void DocumentClosed(object sender, EventArgs e)
		{
			Tracer.Verbose("DocumentFactory.DocumentClosed", ((DocumentContent)sender).Title + " closed");

			DocumentDataContext ddc = (DocumentDataContext)((DocumentContent)sender).DataContext;

			WorkspaceService.Instance.AddRecentFile(ddc.FullName);
		}

		#endregion
		
		#region ----------------INTERNALS----------------

		private DocumentContent GetDocument(SupportedDocumentInfo docInfo)
		{
			return GetDocument(docInfo, null, true);
		}

		private DocumentContent GetDocument(SupportedDocumentInfo docInfo, DocumentDataContext context, bool activate)
		{
			//Tracer.Verbose("DocumentFactory:GetDocument", "docName{0}, docTitle{0}, docType{0}, activate{0}", docName, docTitle, docType, activate);

			DocumentContent doc = FindDocument( docInfo, context );

			try
			{
				if (doc == null)
					doc = CreateNewDocument(docInfo, context);

                if (doc != null && activate)
                    _DockManager.ActiveDocument = doc;
			}
			catch (Exception err)
			{
				Tracer.Error("DocumentFactory.GetDocument", err);
			}
			finally
			{
				Tracer.Verbose("DocumentFactory:GetDocument", "END");
			}

			return doc;
		}

		private DocumentContent CreateNewDocument(SupportedDocumentInfo docInfo, DocumentDataContext context)
        {
			//Tracer.Verbose("DocumentFactory:CreateDocument", "docName{0}, docTitle{0}, docType{0}, activate{0}", docName, docTitle, docType, activate);

            DocumentContent doc = null;
            
            try
            {
                doc = (DocumentContent)Activator.CreateInstance(docInfo.DocumentContentType);

                if( doc != null )
                {
					if (string.IsNullOrEmpty(context.FullName))
					{
						doc.Title = string.Format(docInfo.DefaultTitle, docInfo.Counter++);
						doc.ToolTip = doc.Title;
					}
					doc.Name = docInfo.DocumentContentGUID;
                    doc.DataContext = context;

                    doc.Closing += new EventHandler<CancelEventArgs>(DocumentClosing);
                    doc.Closed += new EventHandler(DocumentClosed);

                    _DockManager.MainDocumentPane.Items.Add(doc);
                }
            }
            catch (Exception err)
            {
				Tracer.Error("DocumentFactory.CreateDocument", err);
            }
            finally
            {
				Tracer.Verbose("DocumentFactory:CreateDocument", "END");
            }
            return doc;
        }

		private DocumentContent FindDocument(SupportedDocumentInfo docInfo, DocumentDataContext context)
        {
            IEnumerable<DocumentContent> list = _DockManager.Documents.Where(d => d.Name == docInfo.DocumentContentGUID && d.Title == context.Name);
			if (list.Count() == 1)
				return list.First();
			else
				return null;
        }

		internal void InitSupportedDocuments()
		{
			_SupportedDocuments.Add(new SupportedDocumentInfo()
			{
				CanCreate = false,
				ShortDescription = "Home",
				LongDescription = "Home Page",
				Image = "/ReflectionStudio;component/Resources/Images/16x16/template.png",
				DocumentContentType = typeof(HomeDocument),
                DocumentContentGUID = "_HomeDocument",
				Counter = 0,
				Extension = "*",
				DefaultTitle = "Home"
			});
			_SupportedDocuments.Add(new SupportedDocumentInfo()
			{
				CanCreate = false,
				ShortDescription = "Help",
				LongDescription = "Help Page",
				Image = "/ReflectionStudio;component/Resources/Images/16x16/template.png",
				DocumentContentType = typeof(HelpDocument),
                DocumentContentGUID = "_HelpDocument",
				Counter = 0,
				Extension = "*",
				DefaultTitle = "Help"
			});
			_SupportedDocuments.Add(new SupportedDocumentInfo()
			{
				CanCreate = false,
				ShortDescription = "Project Settings",
				LongDescription = "SQL Query to run against a data source",
				Image = "/ReflectionStudio;component/Resources/Images/16x16/template.png",
				DocumentContentType = typeof(ProjectDocument),
                DocumentContentGUID = "_DocProject",
				Counter = 0,
				Extension = ".rsp",
				DefaultTitle = "Project settings"
			});
			_SupportedDocuments.Add(new SupportedDocumentInfo()
			{
				CanCreate = true,
				ShortDescription = "SQL Query",
				LongDescription = "SQL Query to run against a data source",
				Image = "/ReflectionStudio;component/Resources/Images/16x16/template.png",
				DocumentContentType = typeof(QueryDocument),
                DocumentContentGUID = "_DocSQLQuery",
				Counter = 0,
				Extension = ".sql",
				DefaultTitle = "Query{0}"
			});
			_SupportedDocuments.Add(new SupportedDocumentInfo()
			{
				CanCreate = true,
				ShortDescription = "Diagram",
				LongDescription = "Assembly and class diagram",
				Image = "/ReflectionStudio;component/Resources/Images/16x16/template.png",
				DocumentContentType = typeof(ProjectDocument),
                DocumentContentGUID = "_DocDiagram",
				Counter = 0,
				Extension = ".rsd",
				DefaultTitle = "Diagram{0}"
			});
			_SupportedDocuments.Add(new SupportedDocumentInfo()
			{
				CanCreate = true,
				ShortDescription = "Template",
				LongDescription = "CSharp template for code generation",
				Image = "/ReflectionStudio;component/Resources/Images/16x16/template.png",
				DocumentContentType = typeof(TemplateDocument),
                DocumentContentGUID = "_DocTemplate",
				Counter = 0,
				Extension = ".rst",
				DefaultTitle = "Template{0}"
			});
		}

		#endregion
	}

	public class SupportedDocumentInfo
	{
		/// <summary>
		/// Display or not in new doc list
		/// </summary>
		public bool CanCreate { get; set; }

        /// <summary>
        /// Icon to display in the list
        /// </summary>
		public string Image { get; set; }

        /// <summary>
        /// Document type
        /// </summary>
		public string ShortDescription { get; set; }

        /// <summary>
        /// Document type description
        /// </summary>
		public string LongDescription { get; set; }

        /// <summary>
        /// Net Document type for creation
        /// </summary>
		public Type DocumentContentType { get; set; }

        /// <summary>
        /// Name of the user control / unique Id
        /// </summary>
		public string DocumentContentGUID { get; set; }

        /// <summary>
        /// Counter used to increment new document without title 
        /// </summary>
		public int Counter { get; set; }

        /// <summary>
        /// default document extension
        /// </summary>
		public string Extension { get; set; }

        /// <summary>
        /// Default title or filename
        /// </summary>
		public string DefaultTitle { get; set; }
	}

    public class DocumentDataContext
    {
		public object Entity
		{
			get;
			set;
		}

		public string FullName
		{
			get;
			set;
		}

		public string NameWithoutExtension
		{
			get
			{
				if (!string.IsNullOrEmpty(FullName))
					return Path.GetFileNameWithoutExtension(FullName);
				else
					return string.Empty;
			}
		}

		public string Name
		{
			get
			{
				if (!string.IsNullOrEmpty(FullName))
					return Path.GetFileName(FullName);
				else
					return string.Empty;
			}
		}

		public string Extension
		{
			get
			{
				if (!string.IsNullOrEmpty(FullName))
					return Path.GetExtension(FullName);
				else
					return string.Empty;
			}
		}
    }

	public class TemplateDocumentDataContext : DocumentDataContext
	{

	}

	public class QueryDocumentDataContext : DocumentDataContext
	{

	}
}
