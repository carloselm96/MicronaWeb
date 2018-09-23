var randomScalingFactor = function () {
    return Math.round(Math.random() * 100);
};

window.chartColors = {
    red: 'rgb(255, 99, 132)',
    orange: 'rgb(255, 159, 64)',
    yellow: 'rgb(255, 205, 86)',
    green: 'rgb(75, 192, 192)',
    blue: 'rgb(54, 162, 235)',
    purple: 'rgb(153, 102, 255)',
    grey: 'rgb(201, 203, 207)'
};

$.ajax({
    cache: false,
    url: '/Home/getPieData',
    type: 'GET',    
    success: function (result) {                
        var datat = [];
        var labelt = [];
        for (var i = 0; i < result.length; i++){
            labelt.push(result[i].label);
            datat.push(result[i].value);
        }
        var config = {
            type: 'doughnut',
            data: {
                datasets: [{
                    data: datat,
                    backgroundColor: [
                        window.chartColors.red,
                        window.chartColors.orange,
                        window.chartColors.yellow,
                        window.chartColors.green,
                        window.chartColors.blue,
                    ],
                    label: 'Dataset 1'
                }],
                labels: labelt
            },
            options: {
                responsive: true,
                legend: {
                    position: 'top',
                },
                title: {
                    display: true,
                    text: 'Distribución de material'
                },
                animation: {
                    animateScale: true,
                    animateRotate: true
                }
            }
        };
        var ctx = document.getElementById('chart-area').getContext('2d');
        window.myDoughnut = new Chart(ctx, config);
    }
})



window.onload = function () {
    
};
