/*
 * 
 npm install --save gulp gulp-plumber gulp-changed gulp-minify-html gulp-autoprefixer gulp-minify-css gulp-uglify gulp-imagemin gulp-rename gulp-concat gulp-strip-debug gulp-notify gulp-livereload del gulp-inject gulp-jshint gulp-replace
 *
 */

/*
 * 
 npm uninstall gulp gulp-plumber gulp-changed gulp-minify-html gulp-autoprefixer gulp-minify-css gulp-uglify gulp-imagemin gulp-rename gulp-concat gulp-strip-debug gulp-notify gulp-livereload del gulp-inject gulp-jshint gulp-replace
 * 
 */

var oldVersionNO = "v0.0.0";
var newVersionNo = "v0.0.1";

var localServerBaseUrl = "http://localhost:54652/";
var productionServerBaseUrl = "http://localhost:54652/";

var templateUrlDevelopmentDirectory = "app/views/";
var templateUrlProductionDirectory = "dist/views/" + newVersionNo + "/";

var footerOldVersion = "Version: 0.0.0";
var footerNewVersion = "Version: 0.0.1";

//var indexTemplateUrlOldDirectory = "dist/views/" + oldVersionNO + "/";
//var indexTemplateUrlNewDirectory = "dist/views/" + newVersionNo + "/";


var gulp = require("gulp"),
    
    changed = require("gulp-changed"),
    imagemin = require("gulp-imagemin"),
    notify = require("gulp-notify"),

	minifyHTML = require("gulp-minify-html"),

	stripDebug = require("gulp-strip-debug"),
	jshint = require("gulp-jshint"),
    plumber = require("gulp-plumber"),
    concat = require("gulp-concat"),
    uglify = require("gulp-uglify"),
        
    autoprefixer = require("gulp-autoprefixer"),
    minifyCSS = require("gulp-minify-css"),
    
	livereload = require("gulp-livereload"),
    del = require("del"),    
    
    inject = require("gulp-inject");

    replace = require("gulp-replace");




// minify new images
gulp.task("images", function () {
    var imgSrc = "./app/images/**/*",
        imgDst = "./dist/images";

    gulp.src(imgSrc)
        .pipe(changed(imgDst))
        .pipe(imagemin())
        .pipe(gulp.dest(imgDst))
        //.pipe(notify({ message: 'images task complete' }))
    ;
});

// minify new or changed HTML pages
gulp.task("htmls", function () {
    var htmlSrc = ["./app/views/**/*.html", "!./app/index.html", "!./app/dev.html"],
        htmlDst = "./dist/views/" + newVersionNo + "/";

    gulp.src(htmlSrc)
        .pipe(changed(htmlDst))
        .pipe(minifyHTML())
        .pipe(gulp.dest(htmlDst))
        //.pipe(notify({ message: 'htmls task complete' }))
    ;
});



// JS concat & uglify 
gulp.task("scripts", function () {

    gulp.src(["./app/scripts/app.config.js","./app/scripts/**/*.config.js"])        
        .pipe(jshint())
        .pipe(jshint.reporter("default"))
        .pipe(plumber())
        .pipe(concat("config-scripts.min.js"))
        .pipe(uglify())
        .pipe(gulp.dest("./dist/scripts/" + newVersionNo + "/"))
        .pipe(notify({ message: 'config scripts task complete' }))
    ;

    gulp.src(["./app/scripts/app.directive.js", "./app/scripts/**/*.directive.js"])
       .pipe(jshint())
       .pipe(jshint.reporter("default"))
       .pipe(plumber())
       .pipe(concat("directive-scripts.min.js"))
       .pipe(uglify())
       .pipe(gulp.dest("./dist/scripts/" + newVersionNo + "/"))
       .pipe(notify({ message: 'directive scripts task complete' }))
    ;

    gulp.src(["./app/scripts/app.service.js", "./app/scripts/**/*.service.js"])
       .pipe(jshint())
       .pipe(jshint.reporter("default"))
       .pipe(plumber())
       .pipe(concat("service-scripts.min.js"))
       .pipe(uglify())
       .pipe(gulp.dest("./dist/scripts/" + newVersionNo + "/"))
       .pipe(notify({ message: 'service scripts task complete' }))
    ;

    gulp.src(["./app/scripts/app.controller.js", "./app/scripts/**/*.controller.js"])
       .pipe(jshint())
       .pipe(jshint.reporter("default"))
       .pipe(plumber())
       .pipe(concat("controller-scripts.min.js"))
       .pipe(uglify())
       .pipe(gulp.dest("./dist/scripts/" + newVersionNo + "/"))
       .pipe(notify({ message: 'controller scripts task complete' }))
    ;
    
});


// CSS concat, auto-prefix and minify
gulp.task("styles", function () {
    gulp.src(["./app/styles/**/*.css"])
        .pipe(concat("style.min.css"))
        .pipe(autoprefixer("last 2 versions"))
        .pipe(minifyCSS())
        .pipe(gulp.dest("./dist/styles/" + newVersionNo + "/"))
        //.pipe(notify({ message: 'styles task complete' }))
    ;
    
});


//replace tasks
gulp.task("replace-templateurl", function () {    
    gulp.src(["./dist/scripts/" + newVersionNo + "/**/*.min.js"])
      .pipe(replace(templateUrlDevelopmentDirectory, templateUrlProductionDirectory))
      .pipe(gulp.dest("./dist/scripts/" + newVersionNo + "/"));        
});

gulp.task("replace-serverurl", function() {
    gulp.src(["./dist/scripts/" + newVersionNo + "/**/*.min.js"])
      .pipe(replace(localServerBaseUrl, productionServerBaseUrl))
      .pipe(gulp.dest("./dist/scripts/" + newVersionNo + "/"));
});

gulp.task("replace-index", function() {
    gulp.src(["./index.html"])
      .pipe(replace(oldVersionNO, newVersionNo))
      .pipe(gulp.dest("./"));

    //gulp.src(["./index.html"])
    //  .pipe(replace(indexTemplateUrlOldDirectory, indexTemplateUrlNewDirectory))
    //  .pipe(gulp.dest("./"));
});

gulp.task("replace-footer-version", function () {
    gulp.src(["./dist/scripts/" + newVersionNo + "/**/*.min.js"])
      .pipe(replace(footerOldVersion, footerNewVersion))
      .pipe(gulp.dest("./dist/scripts/" + newVersionNo + "/"));

    gulp.src(["./app/scripts/app.controller.js"])
      .pipe(replace(footerOldVersion, footerNewVersion))
      .pipe(gulp.dest("./app/scripts/"));
});


// Clean
gulp.task("clean", function (cb) {
    del(["dist/styles", "dist/scripts", "dist/images",  "dist", ".temp"], cb);
    console.log("clean task finished");
});


// Watch
gulp.task("watch", function () {

    // Watch .css files
    gulp.watch("./app/styles/**/*.css", ["styles"]);

    // Watch .js files
    gulp.watch("./app/scripts/**/*.config.js", ["scripts"]);
    gulp.watch("./app/scripts/**/*.directive.js", ["scripts"]);
    gulp.watch("./app/scripts/**/*.service.js", ["scripts"]);
    gulp.watch("./app/scripts/**/*.controller.js", ["scripts"]);

    // Watch .html files
    gulp.watch("./app/**/*.html", ["htmls"]);

    // Watch images files
    gulp.watch("./app/images/**/*", ["images"]);

    // Create LiveReload server
    livereload.listen();

    // Watch any files in dist/, reload on change
    gulp.watch(["./dist/**"]).on("change", livereload.changed);

});

gulp.task("command", function() {
    console.log("clean");
    console.log("default");
    console.log("urls");
    console.log("baseUrl");
    console.log("index");
    console.log("footer");
});

// Default task
gulp.task("default", ["scripts", "styles", "images", "htmls"]);

gulp.task("urls", ["replace-templateurl"]);
gulp.task("baseUrl", ["replace-serverurl"]);
gulp.task("index", ["replace-index"]);
gulp.task("footer", ["replace-footer-version"]);



