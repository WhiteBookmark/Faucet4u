import Vue from "vue";
import "bootstrap-css-only/css/bootstrap.min.css";
import "mdbvue/build/css/mdb.css";
import router from "./routing.js";
import VS2 from "vue-script2";
import { store } from "./store.js";
import VueSocketIO from "vue-socket.io";
import axios from "axios";
import VueAxios from "vue-axios";

import App from "./App.vue";

Vue.use(VueAxios, axios);
Vue.use(VS2);

Vue.use(
  new VueSocketIO({
    debug: true,
    connection: "http://127.0.0.1:3000",
    transports: ["websocket"],
    vuex: {
      store,
      actionPrefix: "SOCKET_",
      mutationPrefix: "SOCKET_"
    }
  })
);

Vue.config.productionTip = true;

new Vue({
  render: h => h(App),
  router,
  store
}).$mount("#app");
