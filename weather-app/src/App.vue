<template>
  <div id="app">
     <nav class="navbar navbar-default navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbar">
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>                        
                </button>
                <a class="navbar-brand">Weather Forecast</a>
            </div>
            <div class="topnav">
                <div class="top-search-container">
                    <select @change="onCityChange()" class="form-control" name="history" v-model="SelectedCity">
                        <option v-for="city in HistoryCities" :value="city.id">{{ city.name }}</option>
                    </select>
                    <span>History</span> 
                </div>
            </div>
        </div>
    </nav>
    
    <div class="container-fluid">
        <div>
            <div class="topnav">
                <div class="search-container">
                    <select aria-placeholder="Select City..." @change="onCityChange()" class="form-control" id="cities" name="character" style="width: 300px" v-model="SelectedCity">
                        <option v-for="(city, index) in Cities" :selected="index === 0" :value="city.id">{{ city.name }}</option>
                    </select>
                </div>
            </div>
        </div>
        <div class="text-left">
            <h2>{{City}} - {{Country}}</h2>
            <h4>{{getTodayString()}}</h4>
        </div>
        <div class="row slideanim">
            <div class="col-md-8">
                <!-- <div id="container"></div> -->
                <div class="btnOuterWrapper">
                    <div class="btnWrapper">
                        <button class="btn temprature" @click="showTempratureChart()">Temprature</button>
                        <div class="arrow-down" v-if="this.VisibleChart===1"></div>
                    </div>
                    <div class="btnWrapper">
                        <button class="btn wind" @click="showWindChart()">Wind</button>
                        <div class="arrow-down" v-if="this.VisibleChart===2"></div>
                    </div>
                    <div class="btnWrapper">
                        <button class="btn humidity" @click="showHumidityChart()">Humidity</button>
                        <div class="arrow-down" v-if="this.VisibleChart===3"></div>
                    </div>
                </div>
                <line-chart :chart-data="ChartData"></line-chart>
            </div>
            <div class="col-md-4">
                <div class="col-md-12" v-for="uniqueDate in UniqueDates">
                    <div class="panel panel-default text-center">
                        <div class="panel-heading">
                            <span>
                                {{getDateTitle(uniqueDate)}}
                            </span>
                        </div>
                        <div class="title">
                            <div class="time"><span>Time</span></div>
                            <div class="temp"><span title="Temprature">°C</span></div>
                            <div class="wind"><span title="Wind">km/h</span></div>
                            <div class="humidity"><span title="Humidity">%</span></div>
                        </div>

                        <div class="panel-body" v-for="weatherInformation in Response.weatherInformation" v-if="isDataInDate(uniqueDate, weatherInformation)==true">
                            <div class="data">
                                <div class="time">{{getHoursFromDate(weatherInformation.epochTime)}}</div>
                                <div class="temp">{{weatherInformation.temprature}}</div>
                                <div class="wind">{{weatherInformation.wind}}</div>
                                <div class="humidity">{{weatherInformation.humidity}}</div>
                            </div>
                        </div>
                    </div>      
                </div> 
            </div> 
        </div>    
    </div>

    <footer class="container-fluid text-center">
        <a title="To Top" id="scrollToTop">
            <span class="glyphicon glyphicon-chevron-up"></span>
        </a>
        <p>Thank you for the opportunity...</p>
    </footer>
  </div>
</template>

<script>
//import HelloWorld from './components/HelloWorld'
import LineChart from '../static/line-chart.js'
import axios from 'axios'

export default {
  name: 'App',
  components: {
    //HelloWorld,
    LineChart
  },
  methods:{
        getCities(){
            axios
            .get('http://localhost:5002/api/city')
            .then(response => {
                this.Cities = response.data;
                this.SelectedCity = response.data[0].id;
                this.getWeatherData(this.SelectedCity);
                this.getHistoryCities();
            })
            .catch(error => alert(error))
        },
        getHistoryCities(){
            var historyCitiesStr = localStorage.cachedHistory;
            if(historyCitiesStr){
                var cachedHistory = JSON.parse(historyCitiesStr);
                this.HistoryCities= Object.keys(cachedHistory).map(function(key){
                    return cachedHistory[key];
                });
            }
        },
        onCityChange(){
            this.getWeatherData(this.SelectedCity);
        },
        getWeatherData(cityId){
            var cachedData = this.getDataFromCache(cityId);
            if (cachedData) {
                this.IsFromCache = true;
                this.updateUI(cachedData);
            }else{
                axios
                .get('http://localhost:5002/api/weather/' + cityId)
                .then(response => {
                    this.Status = response.data.status;
                    this.Message = response.data.message;
                    if(response.data.status == "200"){
                        this.IsFromCache = false;
                        this.updateUI(response.data);
                    }else{
                        alert("Error : " + response.data.message);
                    }
                })
                .catch(error => alert(error))
            }
        },
        updateUI(response){
            this.Response = response;
            this.DateTitlesForChart = response.weatherInformation.map(a => this.getDateTitleAndHoursFromDate(a.epochTime));
            this.City = response.city;
            this.Country = response.country;
            var uniqueDates = [];
            for (var i = 0; i < response.weatherInformation.length; i++) {
                var createdDate = new Date(response.weatherInformation[i].epochTime*1000);
                if (!this.isDateInArray(createdDate, uniqueDates)) {
                    uniqueDates.push(createdDate);
                }
            }
            this.UniqueDates = uniqueDates;
            this.showTempratureChart();
        },
        getDataFromCache(cityId){
            //Cached data for today only
            if(localStorage.cachedDataFor != new Date().getDate()){
                localStorage.removeItem('cachedData');
            }
            var cachedData = localStorage.cachedData;
            if(cachedData){
                var responses = JSON.parse(cachedData);
                return responses[cityId];
            }
        },
        setDataInCache(newResponse){
            //Cache data for the city
            localStorage.cachedDataFor = new Date().getDate();
            var cachedData = localStorage.cachedData;
            // create an empty array
            var responses = {}; 
            // create an empty array
            //If data already exist take it
            if(cachedData){
                responses = JSON.parse(cachedData);
            }
            responses[this.SelectedCity] = newResponse;
            localStorage.cachedData = JSON.stringify(responses);

            //Cache data for the history
            var cachedHistory = localStorage.cachedHistory;
            var history = {}; 
            if(cachedHistory){
                history = JSON.parse(cachedHistory);
            }
            history[this.SelectedCity] = {"id":this.SelectedCity,"name":this.City,"country":this.Country};
            localStorage.cachedHistory = JSON.stringify(history);
            this.getHistoryCities();
        },
        isDateInArray(needle, haystack) {
            for (var i = 0; i < haystack.length; i++) {
                if (needle.setHours(0,0,0,0) === haystack[i].setHours(0,0,0,0)) {
                    return true;
                }
            }
            return false;
        },
        isDataInDate(date, data) {
            return this.isSameDay(date, new Date(data.epochTime * 1000));
        },
        getHoursFromDate(dateInEpoch) {
            var date = new Date(dateInEpoch * 1000);
            //Formating time in two digit
            return ('0'+date.getHours()).substr(-2) + ":" + ('0'+date.getMinutes()).substr(-2);
        },
        getDateTitleAndHoursFromDate(dateInEpoch) {
            var title = "";
            var currentDate = new Date(dateInEpoch * 1000);
            var date = new Date(dateInEpoch * 1000);
            var today = new Date();
            var tomorrow = new Date();
            tomorrow.setDate(today.getDate() + 1);
            if(this.isSameDay(date,today))
            {
                title = "Today";
            }
            else if(this.isSameDay(date,tomorrow)){
                title = "Tomorrow";
            }
            else{
                title = this.Weekday[date.getDay()];
            }
            
            //Formating date title for chart
            return title + " " + ('0'+currentDate.getHours()).substr(-2) + ":" + ('0'+currentDate.getMinutes()).substr(-2);
        },
        getTodayString() {
            var date = new Date();
            var options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
            //Formating the date in local format
            return date.toLocaleDateString("en-US", options);
        },
        getDateTitle(date) {
            var today = new Date();
            var tomorrow = new Date();
            tomorrow.setDate(today.getDate()+1);
            if(this.isSameDay(date,today))
            {
                return "Today";
            }
            else if(this.isSameDay(date,tomorrow)){
                return "Tomorrow";
            }
            else{
                return this.Weekday[date.getDay()];
            }
        },
        isSameDay(date1, date2) {
            //Removing time, just comparing date
            if (date1.setHours(0,0,0,0) === date2.setHours(0,0,0,0)) {
                return true;
            }
            return false;
        },
        showTempratureChart(){
            var tempratureData = this.Response.weatherInformation.map(a => a.temprature.toString());
            this.VisibleChart = 1;
            this.ChartData = {
                labels: this.DateTitlesForChart,
                datasets: [
                {
                    label: 'Temprature',
                    backgroundColor: '#edbf09c7',
                    data: tempratureData
                }]
            }
        },
        showWindChart(){
            var windData = this.Response.weatherInformation.map(a => a.wind.toString());
            this.VisibleChart = 2;
            this.ChartData = {
                labels: this.DateTitlesForChart,
                datasets: [
                {
                    label: 'Wind',
                    backgroundColor: '#2cadda',
                    data: windData
                }]
            }
        },
        showHumidityChart(){
            var humidityData = this.Response.weatherInformation.map(a => a.humidity.toString());
            this.VisibleChart = 3;
            this.ChartData = {
                labels: this.DateTitlesForChart,
                datasets: [
                {
                    label: 'Humidity',
                    backgroundColor: '#1eab83',
                    data: humidityData
                }]
            }
        }
    },
    data:function(){
        return{
            Status: null,
            Message: null,
            SelectedCity: null,
            City: null,
            Cities: [],
            Country: null,
            Response: null,
            Days: null,
            UniqueDates : [],
            Weekday : ["Sunday","Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
            ChartData : null,
            VisibleChart: 1,
            DateTitlesForChart:[],
            IsFromCache:false,
            HistoryCities:false,
        }
    },  
    mounted () {
        this.getCities()
    },
    watch: {
        Response(newResponse) {
            if(!this.IsFromCache)
                this.setDataInCache(newResponse);
        }
    }
}
</script>

<style>
body {font: 400 15px Lato, sans-serif;line-height: 1.8;color: #818181;}
h2 {font-size: 24px;text-transform: uppercase;color: #303030;font-weight: 600;}
h4 {font-size: 19px;line-height: 1.375em;color: #303030;font-weight: 400;}  
.container-fluid {padding: 60px 50px;  }
.panel {border: 1px solid #f4511e; border-radius:0 !important;transition: box-shadow 0.5s;width: 100%}
.panel:hover {box-shadow: 5px 0px 40px rgba(0,0,0, .2);}
.panel-heading {padding: 0;color: #fff !important;background-color: #f4511e !important;border-bottom: 1px solid transparent;
border-top-left-radius: 0px;border-top-right-radius: 0px;border-bottom-left-radius: 0px;border-bottom-right-radius: 0px;}
.panel-heading span{color: white;font-size: 24px}
.panel .title {background: #f4511e;margin: 0;}
.panel .title {background: #f4511e;margin: 0;display: flex;}
.panel .title .time,.panel .panel-body .time{float: left;width: 51%;}
.panel .title .temp,.panel .panel-body .temp{float: left;width: 16.33%;background: #edbf09c7;color:#403f3f}
.panel .title .wind,.panel .panel-body .wind{float: left;width: 16.33%;background: #2cadda;color:#403f3f}
.panel .title .humidity,.panel .panel-body .humidity{float: left;width: 16.33%;background: #1eab83;color:#403f3f}
.panel .title span{color: white;}
.panel .panel-body{border-top: 1px solid #f4511e;padding:0}
.navbar {margin-bottom: 0;background-color: #f4511e;z-index: 9999;border: 0;font-size: 12px !important;
line-height: 1.42857143 !important;letter-spacing: 4px;border-radius: 0;font-family: Montserrat, sans-serif;}
.navbar li a, .navbar .navbar-brand {color: #fff !important;}
.navbar-nav li a:hover, .navbar-nav li.active a {color: #f4511e !important;background-color: #fff !important;}
.navbar-default .navbar-toggle {border-color: transparent;color: #fff !important;}
footer a {cursor:pointer;}
footer .glyphicon {font-size: 20px;margin-bottom: 20px;color: #f4511e;}
.topnav .search-container {float: right;margin-top: 27px;margin-right: 16px;}
.topnav input[type=text] {padding: 6px;margin-top: 0px;font-size: 17px;border: none;}
.topnav .search-container button {color: #f4511e;float: right;padding: 6px 10px;margin-top: 8px;
margin-right: 16px;background: #ddd;font-size: 17px;border: none;cursor: pointer;}
.topnav .search-container button:hover {background: #ccc;}
#cities{width: 250px}
.search-container .sol-container{border: none;box-shadow: none;padding: 0}
.btn.temprature{color:#403f3f;background:#edbf09c7}
.btn.wind{color:#403f3f;background:#2cadda}
.btn.humidity{color:#403f3f;background:#1eab83}
.arrow-down {width: 0; height: 0; border-left: 5px solid transparent;border-right: 5px solid transparent;border-top: 5px solid black;
    margin: 0 auto;}
.btnOuterWrapper{display: inline-block}
.btnWrapper{display: inline-block}
.top-search-container span{float: right;color:white;margin: 16px 3px 0 0}
.top-search-container select{max-width: 250px;float: right;margin-top: 8px;}
</style>