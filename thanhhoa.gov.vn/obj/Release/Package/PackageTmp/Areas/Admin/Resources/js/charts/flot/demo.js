$(function(){

    //
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var d1 = [];
    for (var i = 0; i <= 11; i += 1) {
        d1.push([i, parseInt((Math.floor(Math.random() * (1 + 20 - 10))) + 10)]);
    }
    $("#flot-1ine").length && $.plot($("#flot-1ine"), [{
            data: d1
        }],
        {
            series: {
                lines: {
                    show: true,
                    lineWidth: 2,
                    fill: true,
                    fillColor: {
                        colors: [{
                            opacity: 0.0
                        }, {
                            opacity: 0.2
                        }]
                    }
                },
                points: {
                    radius: 5,
                    show: true
                },
                grow: {
                    active: true,
                    steps: 50
                },
                shadowSize: 2
            },
            grid: {
                hoverable: true,
                clickable: true,
                tickColor: "#f0f0f0",
                borderWidth: 1,
                color: '#f0f0f0'
            },
            colors: ["#65bd77"],
            xaxis:{
            },
            yaxis: {
                ticks: 5
            },
            tooltip: true,
            tooltipOpts: {
                content: "chart: %x.1 is %y.4",
                defaultTheme: false,
                shifts: {
                    x: 0,
                    y: 20
                }
            }
        }
    );

    var d2 = [];
    for (var i = 0; i <= 6; i += 1) {
        d2.push([i, parseInt((Math.floor(Math.random() * (1 + 30 - 10))) + 10)]);
    }
    var d3 = [];
    for (var i = 0; i <= 6; i += 1) {
        d3.push([i, parseInt((Math.floor(Math.random() * (1 + 30 - 10))) + 10)]);
    }
    var d4 = [];
    for (var i = 0; i <= 6; i += 1) {
        d4.push([i, parseInt((Math.floor(Math.random() * (1 + 30 - 10))) + 10)]);
    }
    var d5 = [];
    for (var i = 0; i <= 6; i += 1) {
        d5.push([i, parseInt((Math.floor(Math.random() * (1 + 30 - 10))) + 10)]);
    }
    $("#flot-chart").length && $.plot($("#flot-chart"), [{
            data: d2,
            label: "P"
        }, {
            data: d3,
            label: "U"
        }, {
            data: d4,
            label: "I"
        }, {
            data: d5,
            label: "cos"
        }],
        {
            series: {
                lines: {
                    show: true,
                    lineWidth: 1,
                    fill: true,
                    fillColor: {
                        colors: [{
                            opacity: 0.2
                        }, {
                            opacity: 0.1
                        }]
                    }
                },
                points: {
                    show: true
                },
                shadowSize: 2
            },
            grid: {
                hoverable: true,
                clickable: true,
                tickColor: "#f0f0f0",
                borderWidth: 0
            },
            colors: ["#dddddd","#89cb4e","#6783b7","#8dd168"],
            xaxis: {
                ticks: 15,
                tickDecimals: 0
            },
            yaxis: {
                ticks: 10,
                tickDecimals: 0
            },
            tooltip: true,
            tooltipOpts: {
                content: "'%s' of %x.1 is %y.4",
                defaultTheme: false,
                shifts: {
                    x: 0,
                    y: 20
                }
            }
        }
    );


    // live update
    var data = [],
        totalPoints = 300;

    function getRandomData() {

        if (data.length > 0)
            data = data.slice(1);

        // Do a random walk

        while (data.length < totalPoints) {

            var prev = data.length > 0 ? data[data.length - 1] : 50,
                y = prev + Math.random() * 10 - 5;

            if (y < 0) {
                y = 0;
            } else if (y > 100) {
                y = 100;
            }

            data.push(y);
        }

        // Zip the generated y values with the x values

        var res = [];
        for (var i = 0; i < data.length; ++i) {
            res.push([i, data[i]])
        }

        return res;
    }

    var updateInterval = 30, live;
    $("#flot-live").length && ( live = $.plot("#flot-live", [ getRandomData() ], {
        series: {
            lines: {
                show: true,
                lineWidth: 1,
                fill: true,
                fillColor: {
                    colors: [{
                        opacity: 0.2
                    }, {
                        opacity: 0.1
                    }]
                }
            },
            shadowSize: 2
        },
        colors: ["#cccccc"],
        yaxis: {
            min: 0,
            max: 100
        },
        xaxis: {
            show: false
        },
        grid: {
            tickColor: "#f0f0f0",
            borderWidth: 0
        },
    }) ) && update();

    function update() {

        live.setData([getRandomData()]);

        // Since the axes don't change, we don't need to call plot.setupGrid()

        live.draw();
        setTimeout(update, updateInterval);
    };

    // bar
    var d1_1 = [
        [20, 120],
        [50, 70],
        [80, 100],
        [110, 60]
    ];

    var d1_2 = [
        [20, 80],
        [50, 60],
        [80, 30],
        [110, 35]
    ];

    var d1_3 = [
        [20, 80],
        [50, 40],
        [80, 30],
        [110, 20]
    ];

    var data1 = [
        {
            label: "Bình thường",
            data: d1_1,
            bars: {
                show: true,
                fill: true,
                lineWidth: 0.1,
                order: 0.3,
                fillColor:  "#6783b7"
            },
            color: "#6783b7"
        },
        {
            label: "Cao điểm",
            data: d1_2,
            bars: {
                show: true,
                fill: true,
                lineWidth: 0.1,
                order: 0.5,
                fillColor:  "#4fcdb7"
            },
            color: "#4fcdb7"
        },
        {
            label: "Thấp điểm",
            data: d1_3,
            bars: {
                show: true,
                fill: true,
                lineWidth: 0.1,
                order: 0.7,
                fillColor:  "#8dd168"
            },
            color: "#8dd168"
        }
    ];

    var d2_1 = [
        [90, 10],
        [70, 20],
        [50, 30]
    ];

    var d2_2 = [
        [80, 10],
        [60, 20],
        [40, 30]
    ];

    var d2_3 = [
        [120, 10],
        [50, 20],
        [30, 30]
    ];

    var data2 = [
        {
            label: "Product 1",
            data: d2_1,
            bars: {
                horizontal: true,
                show: true,
                fill: true,
                lineWidth: 1,
                order: 1,
                fillColor:  "#6783b7"
            },
            color: "#6783b7"
        },
        {
            label: "Product 2",
            data: d2_2,
            bars: {
                horizontal: true,
                show: true,
                fill: true,
                lineWidth: 1,
                order: 2,
                fillColor:  "#4fcdb7"
            },
            color: "#4fcdb7"
        },
        {
            label: "Product 3",
            data: d2_3,
            bars: {
                horizontal: true,
                show: true,
                fill: true,
                lineWidth: 1,
                order: 3,
                fillColor:  "#8dd168"
            },
            color: "#8dd168"
        }
    ];

    $("#flot-bar").length && $.plot($("#flot-bar"), data1, {
        xaxis: {

        },
        yaxis: {

        },
        grid: {
            hoverable: true,
            clickable: false,
            borderWidth: 0
        },
        legend: {
            labelBoxBorderColor: "none",
            position: "left"
        },
        series: {
            shadowSize: 1
        },
        tooltip: true,
    });

    $("#flot-bar-h").length && $.plot($("#flot-bar-h"), data2, {
        xaxis: {

        },
        yaxis: {

        },
        grid: {
            hoverable: true,
            clickable: false,
            borderWidth: 0
        },
        legend: {
            labelBoxBorderColor: "none",
            position: "left"
        },
        series: {
            shadowSize: 1
        },
        tooltip: true,
    });

    // pie

    var da = [],
        da1 = [],
        series = Math.floor(Math.random() * 4) + 3;

    for (var i = 0; i < series; i++) {
        da[i] = {
            label: "Series" + (i + 1),
            data: Math.floor(Math.random() * 100) + 1
        }
    }

    for (var i = 0; i < series; i++) {
        da1[i] = {
            label: "Series" + (i + 1),
            data: Math.floor(Math.random() * 100) + 1
        }
    }

    $("#flot-pie-donut").length && $.plot($("#flot-pie-donut"), da1, {
        series: {
            pie: {
                innerRadius: 0.5,
                show: true
            }
        },
        colors: ["#99c7ce","#999999","#bbbbbb","#dddddd","#f0f0f0"],
        grid: {
            hoverable: true,
            clickable: false
        },
        tooltip: true,
        tooltipOpts: {
            content: "%s: %p.0%"
        }
    });

    $("#flot-pie").length && $.plot($("#flot-pie"), da, {
        series: {
            pie: {
                combine: {
                    color: "#999",
                    threshold: 0.05
                },
                show: true
            }
        },
        colors: ["#99c7ce","#999999","#bbbbbb","#dddddd","#f0f0f0"],
        legend: {
            show: false
        },
        grid: {
            hoverable: true,
            clickable: false
        },
        tooltip: true,
        tooltipOpts: {
            content: "%s: %p.0%"
        }
    });
    var dat1=[
        {label:"Điện gia đình",data:41},
        {label:"Điện sản xuất",data:32},
        {label:"Điện kinh doanh",data:27}
    ];

    $("#flot-pie1").length && $.plot($("#flot-pie1"), dat1, {
        series: {
            pie: {
                combine: {
                    color: "#999",
                    threshold: 0.05
                },
                show: true
            }
        },
        colors: ["#99c7ce","#b94a48","#a47e3c"],
        legend: {
            show: false
        },
        grid: {
            hoverable: true,
            clickable: false
        },
        tooltip: true,
        tooltipOpts: {
            content: "%s: %p.0%"
        }
    });

});