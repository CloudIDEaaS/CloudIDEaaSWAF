let path = require('path');
let directives = require("./src/environments/directives.json");
let fs = require('fs');

function load(source, map) {

  let module = this._module;
  let fileName = module.resource ?? "";
  let type = module.type;
  let srcDir = path.join(process.cwd(), "src");
  let callback = this.callback;

  // console.debug(`Processing file, type: ${type}, path: ${fileName}`);

  if (fileName.startsWith(srcDir) && (type == "javascript/auto" || type == "asset/source") && (fileName.endsWith(".ts") || fileName.endsWith(".html?ngResource"))) {

    if (fileName.endsWith(".ts")) {

      let originalSource = source;
      let originalPath = fileName;
      let regexStart = new RegExp("(?<blockIndent>\\s*?)//\\s*?{\\s*?#if\\s*(?<expression>!?\\w*?)\\s*}");
      let regexEnd = new RegExp("//\\s*?{\\s*?#endif\\s*}")
      let rebuild;
      let newSource;

      originalSource = fs.readFileSync(originalPath, "utf8");

      [rebuild, newSource] = handleMatches(originalSource, regexStart, regexEnd, false);

      if (rebuild) {

        console.debug(`!!!Rebuilding file, type: ${type}, path: ${fileName}`);

        fs.writeFileSync(originalPath, newSource, err => {
          if (err) {
            console.error(err);
          } else {
            // file written successfully
          }
        });
      }
    }
    else if (fileName.endsWith(".html?ngResource")) {

      let originalSource = source;
      let originalPath;
      let regexStart = new RegExp("(?<blockIndent>\\s*?)<!--\\s*?{\\s*?#if\\s*(?<expression>!?\\w*?)\\s*?}\\s*?-->");
      let regexEnd = new RegExp("<!--\\s*?{\\s*?#endif\\s*}\\s*?-->")
      let rebuild;
      let newSource;

      fileName = fileName.replace("?ngResource", "");
      originalPath = fileName;

      [rebuild, newSource] = handleMatches(originalSource, regexStart, regexEnd, true);

      if (rebuild) {

        console.debug(`!!!Rebuilding file, type: ${type}, path: ${fileName}`);

        fs.writeFileSync(originalPath, newSource, err => {
          if (err) {
            console.error(err);
          } else {
            // file written successfully
          }
        });
      }
    }
  }

  function hasMatches(source, regexStart) {

    let match = regexStart.exec(source);

    if (match) {
      return true;
    }

    return false;
  }

  function handleMatches(source, regexStart, regexEnd, isHtml) {

    let match = regexStart.exec(source);

    if (match) {

      let lines = source.split(/[\n]+/).map(l => l.replace(/(\r\n|\n|\r)/gm, ""))
      let emitLines = [];
      let commentOut = false;
      let blockIndent;

      lines.forEach(line => {

        let expression;
        let result;

        match = regexStart.exec(line);

        if (match) {
          expression = match.groups.expression;
          blockIndent = match.groups.blockIndent + "  ";
          result = true;

          if (expression.startsWith("!")) {
            expression = expression.substring(1);
            result = false;
          }

          if (directives[expression] == result) {
            commentOut = false;
          }
          else {
            commentOut = true;
          }

          emitLines.push(line);
        }
        else if (regexEnd.exec(line)) {
          emitLines.push(line);
          commentOut = false;
        }
        else {

          let htmlRemoveRegex = new RegExp("\\s*?<!---\\s*?\\(auto added, do not remove\\) ");
          let typescriptRemoveRegex = new RegExp("\\s*?/\\*\\*\\*\\s*?\\(auto added, do not remove\\) ");
          let newLine;

          if (commentOut) {

            if (isHtml) {
              newLine = line.replace(htmlRemoveRegex, "").replace(" --->", "");
              newLine = `${blockIndent}<!--- (auto added, do not remove) ${newLine} --->`

              emitLines.push(newLine);
            }
            else {
              newLine = line.replace(typescriptRemoveRegex, "").replace(" ***/", "");
              newLine = `${blockIndent}/*** (auto added, do not remove) ${newLine} ***/`

              emitLines.push(newLine);
            }
          }
          else {

            if (isHtml) {
              newLine = line.replace(htmlRemoveRegex, "").replace(" --->", "");
              emitLines.push(newLine);
            }
            else {
              newLine = line.replace(typescriptRemoveRegex, "").replace(" ***/", "");
              emitLines.push(newLine);
            }
          }
        }
      });

      return [true, emitLines.join("\r\n")];
    }

    return [false, source];
  }

  callback(null, source, map);
}

module.exports = load;