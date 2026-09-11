namespace Vortexia.Revit.Integration
{
    using Autodesk.Revit.DB;

    /// <summary>
    /// Defines the interface for managing Revit documents.
    /// </summary>
    public interface IDocumentManager
    {
        /// <summary>
        /// Gets the current active Revit document.
        /// </summary>
        Document? CurrentDocument { get; }

        /// <summary>
        /// Gets an element by its ID.
        /// </summary>
        /// <param name="elementId">The element ID.</param>
        /// <returns>The element or null if not found.</returns>
        Element? GetElement(ElementId elementId);

        /// <summary>
        /// Gets all elements of a specific type.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <returns>A collection of elements of the specified type.</returns>
        IEnumerable<T> GetElementsByType<T>() where T : Element;

        /// <summary>
        /// Gets elements by a specific category.
        /// </summary>
        /// <param name="category">The category ID.</param>
        /// <returns>A collection of elements in the specified category.</returns>
        IEnumerable<Element> GetElementsByCategory(ElementId category);

        /// <summary>
        /// Checks if a document is active.
        /// </summary>
        /// <returns>True if a document is active; otherwise, false.</returns>
        bool IsDocumentActive();

        /// <summary>
        /// Gets the project information.
        /// </summary>
        /// <returns>The project information element or null.</returns>
        ProjectInfo? GetProjectInfo();
    }

    /// <summary>
    /// Default implementation of the document manager.
    /// </summary>
    public class DocumentManager : IDocumentManager
    {
        private readonly Document _document;

        /// <summary>
        /// Initializes a new instance of the DocumentManager.
        /// </summary>
        /// <param name="document">The Revit document.</param>
        public DocumentManager(Document document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
        }

        /// <summary>
        /// Gets the current active Revit document.
        /// </summary>
        public Document? CurrentDocument => _document;

        /// <summary>
        /// Gets an element by its ID.
        /// </summary>
        public Element? GetElement(ElementId elementId)
        {
            if (elementId == null || elementId == ElementId.InvalidElementId)
            {
                return null;
            }

            return _document.GetElement(elementId);
        }

        /// <summary>
        /// Gets all elements of a specific type.
        /// </summary>
        public IEnumerable<T> GetElementsByType<T>() where T : Element
        {
            return new FilteredElementCollector(_document)
                .OfClass(typeof(T))
                .Cast<T>();
        }

        /// <summary>
        /// Gets elements by a specific category.
        /// </summary>
        public IEnumerable<Element> GetElementsByCategory(ElementId category)
        {
            return new FilteredElementCollector(_document)
                .OfCategoryId(category)
                .WhereElementIsNotElementType();
        }

        /// <summary>
        /// Checks if a document is active.
        /// </summary>
        public bool IsDocumentActive()
        {
            return _document != null && !_document.IsLinked;
        }

        /// <summary>
        /// Gets the project information.
        /// </summary>
        public ProjectInfo? GetProjectInfo()
        {
            return _document.ProjectInformation;
        }
    }
}
