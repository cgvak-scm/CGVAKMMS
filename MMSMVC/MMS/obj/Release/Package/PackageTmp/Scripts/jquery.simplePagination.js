

(function ($) {

    $.fn.simplePagination = function (options) {

        var defaults = {
            perPage: 20,
            containerClass: '',
            previousButtonClass: 'btn btn-primary btn-sm',
            nextButtonClass: 'btn btn-primary btn-sm',
            previousButtonText: 'Previous',
            nextButtonText: 'Next',
            currentPage: 1
        };

        var settings = $.extend({}, defaults, options);

        return this.each(function () {
            var $rows = $('tbody tr', this);
            var pages = Math.ceil($rows.length / settings.perPage);

            var container = document.createElement('div');
            var bPrevious = document.createElement('button');
            var bNext = document.createElement('button');
            var of = document.createElement('span');

            bPrevious.innerHTML = settings.previousButtonText;
            bNext.innerHTML = settings.nextButtonText;

            container.className = "Paginate";
            bPrevious.className = settings.previousButtonClass;
            bNext.className = settings.nextButtonClass;

            bPrevious.style.marginRight = '8px';

            bNext.style.marginLeft = '8px';
            container.style.textAlign = "center";
            container.style.marginBottom = '20px';

            container.appendChild(bPrevious);
            container.appendChild(of);
            container.appendChild(bNext);

            $(this).after(container);

            update();

            $(bNext).click(function (e) {
                e.preventDefault();
                if (settings.currentPage + 1 > pages) {
                    settings.currentPage = pages;
                } else {
                    settings.currentPage++;
                }

                update();
            });

            $(bPrevious).click(function (e) {
                e.preventDefault();
                if (settings.currentPage - 1 < 1) {
                    settings.currentPage = 1;
                } else {
                    settings.currentPage--;
                }

                update();
            });

            function update() {
                var from = ((settings.currentPage - 1) * settings.perPage);
                var to = from + settings.perPage;

                if (to > $rows.length) {
                    to = $rows.length;
                }

                $rows.hide();
                $rows.slice(from, to).show();



                //  of.innerHTML = (from + 1) + ' to ' + (to) + ' of ' + ($rows.length) + ' entries';

                if ($rows.length == 1) {
                    if ($rows.find("td").length == 1) {
                        of.innerHTML = '0 to 0 of 0 entries';
                    }
                    else
                    {
                        of.innerHTML = (from + 1) + ' to ' + (to) + ' of ' + ($rows.length) + ' entries';
                    }
                }
                else {
                    of.innerHTML = (from + 1) + ' to ' + (to) + ' of ' + ($rows.length) + ' entries';
                }


                //if ((from = 0) || (to = 0)){ //|| $rows.length
                //if ((from = 0) || (to=0)) {
                //    of.innerHTML = (from - 1) + ' to ' + (to - 1) + ' of ' + ($rows.length - 1) + ' entries';
                //}
                //else {
                //    of.innerHTML = (from + 1) + ' to ' + (to) + ' of ' + ($rows.length) + ' entries';
                //}






                if ($rows.length <= settings.perPage) {
                    //$(container).hide();
                    $(container).show();
                } else {
                    $(container).show();
                }
            }
        });

    }

}(jQuery));
