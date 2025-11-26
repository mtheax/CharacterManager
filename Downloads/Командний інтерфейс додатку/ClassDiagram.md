# Class Diagram

Document (Composite) <>-- CompositeElement : inherits
Paragraph (Composite) <>-- CompositeElement
ListElement (Composite) <>-- CompositeElement
ListItem (Composite) <>-- CompositeElement
CompositeElement -- uses --> TextElement (base)

TextRun --|> TextElement

IRenderer <|.. PlainTextRenderer
IRenderer <|.. HtmlRenderer

StructuredTextBuilder --|> uses Document
StructuredTextBuilder --|> uses IRenderer
