using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;

namespace HtmlTags
{

    public class HtmlTag : ITagSource
#if !LEGACY
        , IHtmlString
#endif
    {
        public static HtmlTag Empty()
        {
            return new HtmlTag("span").Render(false);
        }

        public static HtmlTag Placeholder()
        {
            return new HtmlTag().NoTag();
        }

        private const string CssClassAttribute = "class";
        private const string CssStyleAttribute = "style";
        private const string DataPrefix = "data-";
        private static string _metadataSuffix = "__";
        public static void UseMetadataSuffix(string suffix)
        {
            _metadataSuffix = suffix;
        }
        public static string MetadataAttribute { get { return DataPrefix + _metadataSuffix; } }

        private readonly List<HtmlTag> _children = new List<HtmlTag>();
        private readonly HashSet<string> _cssClasses = new HashSet<string>();
        private readonly IDictionary<string, string> _customStyles = new Dictionary<string, string>();

        private readonly Cache<string, HtmlAttribute> _htmlAttributes = new Cache<string, HtmlAttribute>(key => null);

        private readonly Cache<string, object> _metaData = new Cache<string, object>();
        private string _innerText = string.Empty;
        private bool _shouldRender = true;
        private string _tag;
    	private bool _ignoreOpeningTag;
        private bool _ignoreClosingTag;
        private bool _isAuthorized = true;

        private HtmlTag() { }

        public HtmlTag(string tag) : this()
        {
            _tag = tag.ToLower();
        }

        public HtmlTag(string tag, Action<HtmlTag> configure)
            : this(tag)
        {
            configure(this);
        }

        public HtmlTag(string tag, HtmlTag parent) : this(tag)
        {
            if (parent != null) parent.Append(this);
        }

        private bool _encodeInnerText = true;
        
        private HtmlTag _next;

        /// <summary>
        /// The sibling tag that immediately follows the current tag. 
        /// Setting this value will remove any existing value. Use <see cref="After(HtmlTag)"/> if you wish to insert a new sibling before any existing sibling.
        /// </summary>
        public HtmlTag Next { get { return _next; } set { _next = value; } }

        /// <summary>
        /// Inserts a sibling tag immediately after the current tag. Any existing sibling will follow the inserted tag.
        /// </summary>
        /// <param name="nextTag">The tag to add as a sibling</param>
        /// <returns>The original tag</returns>
        public HtmlTag After(HtmlTag nextTag)
        {
            nextTag.Next = _next;
            _next = nextTag;
            return this;
        }

        /// <summary>
        /// Returns the sibling tag that immediately follows the current tag. Same as <see cref="Next" />.
        /// </summary>
        /// <returns></returns>
        public HtmlTag After()
        {
            return _next;
        }

        public IList<HtmlTag> Children { get { return _children; } }


        IEnumerable<HtmlTag> ITagSource.AllTags()
        {
            yield return this;
        }

        public string TagName()
        {
            return _tag;
        }

        public HtmlTag TagName(string tag)
        {
            _tag = tag.ToLower();
            return this;
        }

        public HtmlTag FirstChild()
        {
            return _children.FirstOrDefault();
        }

        public void InsertFirst(HtmlTag tag)
        {
            _children.Insert(0, tag);
        }

        public HtmlTag Style(string key, string value)
        {
            _customStyles[key] = value;
            return this;
        }

        public string Style(string key)
        {
            return _customStyles[key];
        }

        public bool HasStyle(string key)
        {
            return _customStyles.ContainsKey(key);
        }

        public HtmlTag Id(string id)
        {
            return Attr("id", id);
        }

        public string Id()
        {
            return Attr("id");
        }

        public HtmlTag Hide()
        {
            return Style("display", "none");
        }

        /// <summary>
        /// Creates nested child tags and returns the innermost tag. Use <see cref="Append(string)"/> if you want to return the parent tag.
        /// </summary>
        /// <param name="tagNames">One or more HTML element names separated by a <c>/</c> or <c>></c></param>
        /// <returns>The innermost tag that was newly added</returns>
        public HtmlTag Add(string tagNames)
        {
            var tags = tagNames.ToDelimitedArray('/', '>');
            return tags.Aggregate(this, (parent, tag) => new HtmlTag(tag, parent));
        }

        /// <summary>
        /// Creates nested child tags and returns the innermost tag after running <paramref name="configuration"/> on it. Use <see cref="Append(string, Action{HtmlTag})"/> if you want to return the parent tag.
        /// </summary>
        /// <param name="tagNames">One or more HTML element names separated by a <c>/</c> or <c>></c></param>
        /// <param name="configuration">Modifications to perform on the newly added innermost tag</param>
        /// <returns>The innermost tag that was newly added</returns>
        public HtmlTag Add(string tagNames, Action<HtmlTag> configuration)
        {
            var element = Add(tagNames);
            configuration(element);

            return element;
        }

        /// <summary>
        /// Creates a tag of <typeparamref name="T"/> and adds it as a child. Returns the created child tag.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="HtmlTag"/> to create</typeparam>
        /// <returns>The created child tag</returns>
        public T Add<T>() where T : HtmlTag, new()
        {
            var child = new T();
            _children.Add(child);

            return child;
        }

        /// <summary>
        /// Adds the given tag as the last child of the parent, and returns the parent.
        /// </summary>
        /// <param name="child">The tag to add as a child of the parent.</param>
        /// <returns>The parent tag</returns>
        public HtmlTag Append(HtmlTag child)
        {
            _children.Add(child);
            return this;
        }

        /// <summary>
        /// Creates nested child tags and returns the tag on which the method was called. Use <see cref="Add(string)"/> if you want to return the innermost tag.
        /// </summary>
        /// <param name="tagNames">One or more HTML element names separated by a <c>/</c> or <c>></c></param>
        /// <returns>The instance on which the method was called (the parent of the new tags)</returns>
        public HtmlTag Append(string tagNames)
        {
            Add(tagNames);
            return this;
        }

        /// <summary>
        /// Creates nested child tags, runs <paramref name="configuration"/> on the innermost tag, and returns the tag on which the method was called. Use <see cref="Add(string, Action{HtmlTag})"/> if you want to return the innermost tag.
        /// </summary>
        /// <param name="tagNames"></param>
        /// <param name="configuration"></param>
        /// <returns>The parent tag</returns>
        public HtmlTag Append(string tagNames, Action<HtmlTag> configuration)
        {
            Add(tagNames, configuration);
            return this;
        }

        /// <summary>
        /// Adds all tags from <paramref name="tagSource"/> as children of the current tag. Returns the parent tag.
        /// </summary>
        /// <param name="tagSource">The source of tags to add as children.</param>
        /// <returns>The parent tag</returns>
        public HtmlTag Append(ITagSource tagSource)
        {
            _children.AddRange(tagSource.AllTags());
            return this;
        }

        /// <summary>
        /// Adds a sequence of tags as children of the current tag. Returns the parent tag.
        /// </summary>
        /// <param name="tags">A sequence of tags to add as children.</param>
        /// <returns>The parent tag</returns>
        public HtmlTag Append(IEnumerable<HtmlTag> tags)
        {
            _children.AddRange(tags);
            return this;
        }

        /// <summary>
        /// Stores a value in an HTML5 custom data attribute
        /// </summary>
        /// <param name="key">The name of the data attribute. Will have "data-" prepended when rendered.</param>
        /// <param name="value">The value to store. Non-string values will be JSON </param>
        /// <returns>The calling tag.</returns>
        public HtmlTag Data(string key, object value)
        {
            var dataKey = DataPrefix + key;
            if (value == null)
            {
                return RemoveAttr(dataKey);
            }
            _htmlAttributes[dataKey] = new HtmlAttribute(value);
            return this;
        }

        /// <summary>
        /// Modifies an existing reference value stored in an HTML5 custom data
        /// </summary>
        /// <typeparam name="T">The type of the data stored in the given location</typeparam>
        /// <param name="key">The name of the data storage location</param>
        /// <param name="configure">The action to perform on the currently stored value</param>
        /// <returns>The calling tag.</returns>
        public HtmlTag Data<T>(string key, Action<T> configure) where T : class
        {
            var dataKey = DataPrefix + key;
            if (!_htmlAttributes.Has(dataKey)) return this;
            var value = (T)_htmlAttributes[dataKey].Value;
            configure(value);
            return this;
        }

        /// <summary>
        /// Returns the value stored in HTML5 custom data
        /// </summary>
        /// <param name="key">The name of the data storage location</param>
        /// <returns>The calling tag.</returns>
        public object Data(string key)
        {
            var dataKey = DataPrefix + key;
            if (!_htmlAttributes.Has(dataKey)) return null;
            return _htmlAttributes[dataKey].Value;
        }

        /// <summary>
        /// Stores multiple JSON-encoded key/value pairs in a the "data-__" attribute. Useful when used with the jquery.metadata plugin
        /// </summary>
        /// <param name="key">The name of the stored value</param>
        /// <param name="value">The value to store</param>
        /// <remarks>You need to configure the the jquery.metadata plugin to read from the data-__ attribute.
        /// Add the following line after you have loaded jquery.metadata.js, but before you use its metadata() method:
        /// <code>
        /// if ($.metadata) {
        ///    $.metadata.setType('attr', 'data-__');
        /// }
        /// </code>
        /// </remarks>
        /// <returns>The calling tag.</returns>
        public HtmlTag MetaData(string key, object value)
        {
            _metaData[key] = value;
            return this;
        }

        /// <summary>
        /// Modifies an existing reference value stored in MetaData
        /// </summary>
        /// <typeparam name="T">The type of the stored value</typeparam>
        /// <param name="key">The name of the stored value</param>
        /// <param name="configure">The action to perform on the currently stored value</param>
        /// <returns>The calling tag.</returns>
        public HtmlTag MetaData<T>(string key, Action<T> configure) where T : class
        {
            if (!_metaData.Has(key)) return this;
            var value = (T)_metaData[key];
            configure(value);

            return this;
        }

        /// <summary>
        /// Returns the MetaData value stored for a given key.
        /// </summary>
        /// <param name="key">The name of the stored value</param>
        /// <returns>The calling tag.</returns>
        public object MetaData(string key)
        {
            return !_metaData.Has(key) ? null : _metaData[key];
        }

        public HtmlTag Text(string text)
        {
            _innerText = text;
            return this;
        }

        public string Text()
        {
            return _innerText;
        }


        public HtmlTag Modify(Action<HtmlTag> action)
        {
            action(this);
            return this;
        }

        public HtmlTag Authorized(bool isAuthorized)
        {
            _isAuthorized = isAuthorized;
            return this;
        }

        public bool Authorized()
        {
            return _isAuthorized;
        }


        public override string ToString()
        {
            return WillBeRendered() ? 
                ToString(new HtmlTextWriter(new StringWriter(), string.Empty) { NewLine = string.Empty }) :
                string.Empty;
        }

        public string ToHtmlString()
        {
            return ToString();
        }

        public string ToPrettyString()
        {
            return WillBeRendered() ?
                ToString(new HtmlTextWriter(new StringWriter(), "  ") { NewLine = Environment.NewLine }) :
                string.Empty;
        }


        public string ToString(HtmlTextWriter html)
        {
            writeHtml(html);
            return html.InnerWriter.ToString();
        }

        public bool WillBeRendered()
        {
            return _shouldRender && _isAuthorized;
        }

        protected virtual void writeHtml(HtmlTextWriter html)
        {
            if (!WillBeRendered()) return;

			if (HasTag())
			{
            _htmlAttributes.Each((key, attribute) =>
				                     	{
                if (attribute != null)
				                     		{
                    var value = attribute.Value;
                    var stringValue = !(value is string) && key.StartsWith(DataPrefix) ? JsonUtil.ToJson(value) : value.ToString();
                    html.AddAttribute(key, stringValue, attribute.IsEncoded);
				                     		}
				                     	});

				if (_cssClasses.Count > 0)
				{
					var classValue = _cssClasses.Join(" ");
					html.AddAttribute(CssClassAttribute, classValue);
				}

				if (_metaData.Count > 0)
				{
					var metadataValue = JsonUtil.ToUnsafeJson(_metaData.Inner);
					html.AddAttribute(MetadataAttribute, metadataValue);
				}

				if (_customStyles.Count > 0)
				{
					var attValue = _customStyles
						.Select(x => x.Key + ":" + x.Value)
						.ToArray().Join(";");

					html.AddAttribute(CssStyleAttribute, attValue);
				}

				html.RenderBeginTag(_tag);
			}

        	writeInnerText(html);

            _children.Each(x => x.writeHtml(html));

            if (HasClosingTag())
            {
                html.RenderEndTag();
            }

            if (_next != null) _next.writeHtml(html);
        }

        private void writeInnerText(HtmlTextWriter html)
        {
            if (_innerText == null) return;

            if (_encodeInnerText)
            {
                html.WriteEncodedText(_innerText);
            }
            else
            {
                html.Write(_innerText);
            }
        }

        public HtmlTag Attr(string attribute, object value)
        {
            return buildAttr(attribute, value);
        }

        public HtmlTag UnencodedAttr(string attribute, object value)
        {
            return buildAttr(attribute, value, false);
        }

        private HtmlTag buildAttr(string attribute, object value, bool encode = true)
        {
            if (value == null || value.Equals(string.Empty))
            {
                return RemoveAttr(attribute);
            }
            if (isCssClassAttr(attribute))
            {
                AddClass(value.ToString());
            }   
            else
            {
                _htmlAttributes[attribute] = new HtmlAttribute(value.ToString(), encode);
            }
            return this;
        }

        private static bool isCssClassAttr(string attribute)
        {
            return attribute.Equals(CssClassAttribute, StringComparison.OrdinalIgnoreCase);
        }

        private static bool isCssStyleAttr(string attribute)
        {
            return attribute.Equals(CssStyleAttribute, StringComparison.OrdinalIgnoreCase);
        }

        private static bool isMetadataAttr(string attribute)
        {
            return attribute.Equals(MetadataAttribute, StringComparison.OrdinalIgnoreCase);
        }

        public string Attr(string attribute)
        {
            var attrVal = _htmlAttributes[attribute];
            return attrVal == null ? string.Empty : attrVal.ToString();
        }

        public HtmlTag RemoveAttr(string attribute)
        {
            if (isCssClassAttr(attribute))
            {
                _cssClasses.Clear();
            }
            else if (isCssStyleAttr(attribute))
            {
                _customStyles.Clear();
            }
            else if (isMetadataAttr(attribute))
            {
                _metaData.ClearAll();
            }
            else
            {
                _htmlAttributes.Remove(attribute);
            }
            return this;
        }

        public HtmlTag Render(bool shouldRender)
        {
            _shouldRender = shouldRender;
            return this;
        }

        public bool Render()
        {
            return _shouldRender;
        }

        public HtmlTag AddClass(string className)
        {
            IEnumerable<string> classes = parseClassName(className);
            foreach (string parsedClass in classes)
            {
                if (isInvalidClassName(parsedClass))
                {
                    throw new ArgumentException("CSS class names is not valid. Problem class was '{0}'".ToFormat(className), "className");
                }

                _cssClasses.Add(parsedClass);
            }

            return this;
        }

        private static bool isJsonClassName(string className)
        {
            return className.StartsWith("{") && className.EndsWith("}")
                    || className.StartsWith("[") && className.EndsWith("]");
        }

        private static bool isInvalidClassName(string className)
        {
            bool valid;
            if (isJsonClassName(className))
            {
                // Allow JSON blobs.
                // http://stackoverflow.com/questions/7256142/way-to-quickly-check-if-string-is-xml-or-json-in-c-sharp 

                valid = true;
            }
            else
            {
                /*
                 * Basically, a name must begin with an underscore (_), a dash (-), or a letter(a�z), 
                 * followed by any number of dashes, underscores, letters, or numbers. 
                 * There is a catch: if the first character is a dash, the second character must be a letter or underscore, 
                 * and the name must be at least 2 characters long.
                 * 
                 * http://stackoverflow.com/questions/448981/what-characters-are-valid-in-css-class-names
                 */

                valid = Regex.IsMatch(className, @"^-?[_a-zA-Z]+[_a-zA-Z0-9-]*$");
            }

            return !valid;
        }

        /// <summary>
        /// Parses a string which contains class name or multiple class names.
        /// </summary>
        /// <param name="className">A string which contains class(-es)</param>
        /// <returns>The list of classes</returns>
        private static IEnumerable<string> parseClassName(string className)
        {
            IEnumerable<string> classes;
            if (isJsonClassName(className))
            {
                classes = new List<string> { className };
            }
            else
            {
                classes = Regex.Split(className, "[ ]+")
                               .Where(c => !string.IsNullOrWhiteSpace(c));
            }

            return classes;
        }

        public HtmlTag AddClasses(params string[] classes)
        {
            return addClasses(classes);
        }

        public HtmlTag AddClasses(IEnumerable<string> classes)
        {
            return addClasses(classes);
        }

        private HtmlTag addClasses(IEnumerable<string> classes)
        {
            foreach (var cssClass in classes)
            {
                AddClass(cssClass);
            }
            return this;
        }

        public IEnumerable<string> GetClasses()
        {
            return _cssClasses;
        }

        public bool HasClass(string className)
        {
            return _cssClasses.Contains(className);
        }

        public HtmlTag RemoveClass(string className)
        {
            _cssClasses.Remove(className);
            return this;
        }

        public bool HasMetaData(string key)
        {
            return _metaData.Has(key);
        }

        public string Title()
        {
            return Attr("title");
        }

        public HtmlTag Title(string title)
        {
            return Attr("title", title);
        }

        public bool HasAttr(string key)
        {
            if (isCssClassAttr(key)) return _cssClasses.Count > 0;
            if (isCssStyleAttr(key)) return _customStyles.Count > 0;
            if (isMetadataAttr(key)) return _metaData.Count > 0;
            return _htmlAttributes.Has(key);
        }

        public bool IsInputElement()
        {
            return _tag == "input" || _tag == "select" || _tag == "textarea";
        }

        public void ReplaceChildren(params HtmlTag[] tags)
        {
            Children.Clear();
            tags.Each(t => Children.Add(t));
        }

		/// <summary>
		/// Specify that the tag should render only its children and not itself.  
		/// Used for declaring container/placeholder tags that should not affect the final markup.
		/// </summary>
		/// <returns></returns>
		public HtmlTag NoTag()
		{
			_ignoreOpeningTag = true;
			_ignoreClosingTag = true;
			return this;
		}

        public HtmlTag NoClosingTag()
        {
            _ignoreClosingTag = true;
            return this;
        }

		/// <summary>
		/// Get whether or not to render the tag itself or just the children of the tag. 
		/// </summary>
		/// <returns></returns>
		public bool HasTag()
		{
			return !_ignoreOpeningTag;
		}

        public bool HasClosingTag()
        {
            return !_ignoreClosingTag;
        }

        public HtmlTag WrapWith(string tag)
        {
            var wrapper = new HtmlTag(tag);
            wrapper.Append(this);

            // Copies visibility and authorization from inner tag
            wrapper.Render(Render());
            wrapper.Authorized(Authorized());

            return wrapper;
        }

        public HtmlTag WrapWith(HtmlTag wrapper)
        {
            wrapper.InsertFirst(this);
            return wrapper;
        }

        public HtmlTag VisibleForRoles(params string[] roles)
        {
            var principal = findPrincipal();
            return Render(roles.Any(principal.IsInRole));
        }

        private static IPrincipal findPrincipal()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.User;
            }

            // Rather throw up on nulls than put a fake in
            return Thread.CurrentPrincipal;
        }

        public HtmlTag Encoded(bool encodeInnerText)
        {
            _encodeInnerText = encodeInnerText;
            return this;
        }

        public bool Encoded()
        {
            return _encodeInnerText;
        }

        private class HtmlAttribute
        {
            public HtmlAttribute(object value)
                : this(value, true)
            {
            }

            public HtmlAttribute(object value, bool isEncoded)
            {
                Value = value;
                IsEncoded = isEncoded;
            }

            public object Value { get; set; }
            public bool IsEncoded { get; set; }

            public override string ToString()
            {
                return Value != null ? Value.ToString() : string.Empty;
            }
        }
    }
}
