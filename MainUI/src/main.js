import Vue from "vue";

//Make sure you import the store first and then router
import "bootstrap-css-only/css/bootstrap.min.css";
import "mdbvue/build/css/mdb.css";
import "vue-select/dist/vue-select.css";
import '@mdi/font/css/materialdesignicons.css';
import { AgGridVue } from "ag-grid-vue";
import { store } from "@/scripts/store";
import 'animate.css';
import axios from "axios";
import moment from "moment";
import router from "@/scripts/routing";
import SkyscrapperBanner from "@/components/Custom/SkyscrapperBanner";
import SquareBanner from "@/components/Custom/SquareBanner";
import StandardBanner from "@/components/Custom/StandardBanner";
import FloatingSquareBanner from "@/components/Custom/FloatingSquareBanner";
import CaptchaModal from "@/components/Custom/CaptchaModal";
import MessageModal from "@/components/Custom/MessageModal";
import ErrorModal from "@/components/Custom/ErrorModal";
import SuccessModal from "@/components/Custom/SuccessModal";
import ChatRulesModal from "@/components/Custom/ChatRulesModal";
import VeeValidate from "vee-validate";
import VS2 from "vue-script2";
import vSelect from "vue-select";
import VueCookie from "vue-cookie";
import vueMoment from "vue-moment";
import VueSocketIO from "vue-socket.io";
import Vuetify from 'vuetify';
import 'vuetify/dist/vuetify.min.css';
import mixins from '@/scripts/mixins';

import App from "@/App";

Vue.use(VS2);
Vue.use(VueCookie);
Vue.use(VeeValidate);
Vue.use(vueMoment);
Vue.use(Vuetify);
Vue.prototype.moment = moment;
Vue.prototype.$http = axios;
Vue.prototype.axios = axios;

//Vue.use(
//    new VueSocketIO({
//        debug: true,
//        connection: "http://127.0.0.1:3000",
//        transports: ["websocket"]
//    })
//);

Vue.component("v-select", vSelect);
Vue.component("standard-banner", StandardBanner);
Vue.component("square-banner", SquareBanner);
Vue.component("skyscrapper-banner", SkyscrapperBanner);
Vue.component("floating-square-banner", FloatingSquareBanner);
Vue.component("success-modal", SuccessModal);
Vue.component("error-modal", ErrorModal);
Vue.component("message-modal", MessageModal);
Vue.component("captcha-modal", CaptchaModal);
Vue.component("chat-rules-modal", ChatRulesModal);
Vue.component("ag-grid-vue", AgGridVue);
Vue.config.productionTip = true;

Vue.mixin(mixins);

new Vue({
    render: h => h(App),
    router,
    vuetify: new Vuetify(),
    store
}).$mount("#app");
