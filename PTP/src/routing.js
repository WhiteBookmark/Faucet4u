import Vue from "vue";
import Router from "vue-router";

import Index from "../src/components/Index/Index.vue";
import Invisible from "../src/components/Index/Invisible.vue";

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: "/",
      name: "Index",
      component: Index
    },
    {
      path: "/Invisible",
      name: "Invisible",
      component: Invisible
    }
  ]
});
