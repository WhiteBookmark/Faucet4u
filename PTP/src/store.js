import Vue from "vue";
import Vuex from "vuex";
const parseDomain = require("parse-domain");

Vue.use(Vuex);

export const store = new Vuex.Store({
    state: {
        Valid: true,
        AbsoluteReferrer: null,
        Referrer: null,
        Username: null,
        ip: "0.0.0.0"
    },
    mutations: {
        Valid(state, value)
        {
            state.Valid = value;
        },
        AbsoluteReferrer(state, value)
        {
            state.AbsoluteReferrer = value;
        },
        Referrer(state, value)
        {
            var stringedValue = value;
            if (
                stringedValue == null ||
                stringedValue === "" ||
                stringedValue.includes("localhost")
            )
                state.Referrer = value;
            else state.Referrer = parseDomain(value).domain;
        },
        Username(state, value)
        {
            state.Username = value;
        },
        IP(state, value)
        {
            state.ip = value;
        }
    }
});
