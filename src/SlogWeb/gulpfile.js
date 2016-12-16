var gulp = require('gulp'),
    plumber = require("gulp-plumber"),
    sourceMaps = require("gulp-sourcemaps"),
    tsc = require("gulp-typescript"),
    tsLint = require("gulp-tslint"),
    concat = require("gulp-concat"),
    sass = require("gulp-sass"),
    uglify = require("gulp-uglify");

gulp.task("tsc", function () {
    var tsProject = tsc.createProject("tsconfig.json");
    var tsResult = gulp.src("ClientSource/JavaScript/**/*.ts")
        .pipe(plumber())
        .pipe(tsLint({ formatter: "prose", configuration: "tslint.json" }))
        .pipe(tsLint.report({ emitError: false }))
        .pipe(sourceMaps.init())
        .pipe(tsProject());

    return tsResult.js
        .pipe(sourceMaps.write("."))
        .pipe(gulp.dest("wwwroot/js"));
});

gulp.task("copyFonts", function () {
    return gulp.src("node_modules/bootstrap-sass/assets/fonts/bootstra/*.*")
        .pipe(gulp.dest("wwwroot/fonts"));
});

gulp.task("sass", function () {
    return gulp.src("ClientSource/StyleSheets/**/*.scss")
        .pipe(plumber())
        .pipe(sourceMaps.init())
        .pipe(sass({ style: "compressed" }))
        .pipe(sourceMaps.write("."))
        .pipe(gulp.dest("wwwroot/css"));
});

gulp.task("build", ["copyFonts", "sass", "tsc"]);

gulp.task('default', function () {
    // place code for your default task here
});