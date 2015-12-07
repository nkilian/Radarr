const _ = require('underscore');

module.exports = function() {
  window.Sonarr.NameViews = window.Sonarr.NameViews || !window.Sonarr.Production;

  var originalRender = this.render;

  this.render = function() {
    var renderResult = originalRender.apply(this, arguments);

    if (window.Sonarr.NameViews && _.isString(this.template)) {
      this.$el.attr('data-hbs', this.template);
    }

    return renderResult;
  };

  return this;
};