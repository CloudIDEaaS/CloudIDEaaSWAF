let directives = require("./src/environments/directives.json");
let packageJson = require("./package.json");
let version = require("./src/environments/version.json");
let path = require('path');
let fs = require("fs");

module.exports = (grunt) => {

  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),
    copy: {
      build: {
        files: [
        ]
      }
    },
    scram: {
      options: {},
      build: {
        expand: true, src: [
          'bootstrap.bundle.min.js'
        ],
        dest: 'src/pki/prod',
        cwd: 'src/pki/'
      },
    },
    javascript_obfuscator: {
      options: {
        debugProtection: true
      },
      main: {
        files: {
          'src/pki/prod/bootstrap.bundle.min.js': ['src/pki/min/bootstrap.bundle.min.js']
        }
      }
    },
    uglify: {
      main: {
        options: {
          sourceMap: false
        },
        files: {
          'src/pki/min/bootstrap.bundle.min.js': ['src/pki/bootstrap.bundle.min.js']
        }
      }
    },
    bump: {
      options: {
        files: ['package.json'],
        updateConfigs: [],
        commit: false,
        commitMessage: 'Release v%VERSION%',
        commitFiles: ['package.json'],
        createTag: true,
        tagName: 'v%VERSION%',
        tagMessage: 'Version %VERSION%',
        push: false,
        gitDescribeOptions: '--tags --always --abbrev=1 --dirty=-d',
        globalReplace: false,
        prereleaseName: false,
        metadata: '',
        regExp: false
      }
    }
  });

  grunt.loadNpmTasks('grunt-contrib-copy');
  grunt.loadNpmTasks('grunt-bump');
  grunt.loadNpmTasks('grunt-scram');
  grunt.loadNpmTasks('grunt-javascript-obfuscator');
  grunt.loadNpmTasks('grunt-contrib-uglify');

  grunt.registerTask('package', ['copy']);

  grunt.registerTask('prep-environment', 'Updates environment information with specific information.', (configuration) => {

    let versionString = packageJson.version;
    let apiVersionString = packageJson.apiversion;
    let versionJsonFile = path.join(path.dirname(__filename), "src/environments/version.json");
    let directivesJsonFile = path.join(path.dirname(__filename), "src/environments/directives.json");

    version.version = versionString;
    version.apiversion = apiVersionString;

    console.log(`Running prep-environment task, configuration: ${configuration}`);
    console.log(`Setting version to ${version.version}, api version to ${version.apiversion}`);

    fs.writeFileSync(versionJsonFile, JSON.stringify(version), err => {
      if (err) {
        console.error(err);
      } else {
        console.log("Updating version");
      }
    });

    directives.DEBUG = configuration != "production";
    directives.PROD = configuration == "production";

    console.log(`Setting directives DEBUG: ${directives.DEBUG}, PROD: ${directives.PROD}`);

    fs.writeFileSync(directivesJsonFile, JSON.stringify(directives), err => {
      if (err) {
        console.error(err);
      } else {
        console.log("Updating directives");
      }
    });
  });
};
