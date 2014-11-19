#I @"packages/Microsoft.AspNet.Razor/lib"
#I @"packages/RazorEngine/lib/net40"
#I @"packages/FSharp.Formatting/lib/net40/"
#I @"packages/FSharp.Compiler.Service/lib/net45"

#r "System.Web.Razor"
#r "RazorEngine"
#r "FSharp.Literate"
#r "FSharp.CodeFormat"
#r "FSharp.Markdown"
#r "FSharp.MetadataFormat"
#r "FSharp.Compiler.Service"

open System.IO
open FSharp.Literate

// ----------------------------------------------------------------------------
// SETUP
// ----------------------------------------------------------------------------

/// Return path relative to the current file location
let relative subdir = Path.Combine(__SOURCE_DIRECTORY__, subdir)

// Create output directories & copy content files there
// (We have two sets of samples in "output" and "output-all" directories,
//  for simplicitly, this just creates them & copies content there)
if not (Directory.Exists(relative "output")) then
  Directory.CreateDirectory(relative "output") |> ignore

/// Processes a single Markdown document and produce HTML output
let processDocument doc =
  let file = relative (sprintf "Docs/%s.md" doc)
  let output = relative (sprintf "output/%s.html" doc)
  let template = relative "templates/template-file.html"
  Literate.ProcessMarkdown(file, template, output)

let parseDocument doc =
  let file = relative (sprintf "Docs/%s.md" doc)
  Literate.ParseMarkdownFile(file)

/// Processes an entire directory containing multiple script files 
/// (*.fsx) and Markdown documents (*.md) and it specifies additional 
/// replacements for the template file
let processDirectory() =
  let template = relative "templates/template-project.html"
  let projInfo =
    [ "page-description", "F# Scheme"
      "page-author", "Author"
      "github-link", "https://github.com/tpetricek/FSharp.Formatting"
      "project-name", "F# Scheme" ]
  Literate.ProcessDirectory
    ( relative "Docs", template, relative "output", 
      OutputKind.Html, replacements = projInfo,)

processDirectory()
processDocument "intro" 
(parseDocument "intro").Errors |> Seq.toArray