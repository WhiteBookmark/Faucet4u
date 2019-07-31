<template>
  <div>
    <mdb-row>
      <mdb-col col="1">
        <skyscrapper-banner></skyscrapper-banner>
      </mdb-col>
      <mdb-col class="text-center">
        <mdb-row>
          <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
          <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
        </mdb-row>
        <br />
        <br />

        <mdb-row>
          <mdb-col col="1"></mdb-col>
          <mdb-col>
            <mdb-input
              basic
              class="mb-3"
              v-validate="
                `required|min_value:${
                  this.$store.getters.faucetSettingValue('MinimumDeposit').Value
                }`
              "
              name="amount"
              v-model.trim="amount"
              type="number"
            >
              <span class="input-group-text" slot="prepend"
                >Amount in Bitcoin</span
              >
            </mdb-input>
          </mdb-col>
        </mdb-row>

        <span class="font-weight-bold text-danger">{{ errorList }}</span>
        <span class="font-weight-bold text-success">{{ successList }}</span>
        <span class="text-danger"> {{ errors.first("amount") }} <br /> </span>
        <span class="font-weight-bold text-warning"
          >Funds will automatically be deposited in your purchase balance once
          the transaction reaches 6 confirmations.</span
        >

        <mdb-row>
          <mdb-col>
            <mdb-btn v-on:click="deposit()" color="orange" size="lg"
              >Deposit with Bitcoin</mdb-btn
            >
          </mdb-col>
        </mdb-row>

        <span class="font-weight-bold text-info"
          >To deposit with a different method, you can use the exchange option
          below.</span
        >
        <br />
        <span class="font-weight-bold text-info"
          >Just use our bitcoin deposit method to generate a bitcoin address and
          send funds using the exchanger below.</span
        >

        <div v-html="changerHTML"></div>
      </mdb-col>
      <mdb-col col="1">
        <skyscrapper-banner></skyscrapper-banner>
      </mdb-col>
      <mdb-col col="1"></mdb-col>
    </mdb-row>

    <button
      type="button"
      class="bottomLeftButton"
      id="bottomLeftButton"
      onClick="document.getElementById('bottomLeftBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomLeftBanner" id="bottomLeftBanner">
      <square-banner></square-banner>
    </div>

    <button
      type="button"
      class="bottomRightButton"
      id="bottomRightButton"
      onClick="document.getElementById('bottomRightBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomRightBanner" id="bottomRightBanner">
      <square-banner></square-banner>
    </div>
  </div>
</template>

<script>
import { mdbRow, mdbCol, mdbBtn, mdbDatatable, mdbInput } from "mdbvue";

export default {
  components: { mdbRow, mdbCol, mdbBtn, mdbDatatable, mdbInput },
  data() {
    return {
      recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
      recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
      amount: this.$store.getters.faucetSettingValue("MinimumDeposit").Value,
      data: {
        columns: [
          {
            label: "Deposit Method Name",
            field: "name",
            sort: "asc"
          },
          {
            label: "Address",
            field: "address",
            sort: "asc"
          }
        ],
        rows: []
      },
      successList: null,
      errorList: null,
      changerHTML: `<iframe src='http://localhost:8081/Other/Changer.html' scrolling="no" style="width:736px; height:600px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>`
    };
  },
  sockets: {
    //receivingDepositMethod: function (data) {
    //    this.data.rows.length = 0;
    //    for (var i = 0; i < Object.keys(data).length; i++) {
    //        this.data.rows.push({
    //            name: data[i].Name,
    //            address: data[i].Address
    //        });
    //    }
    //    this.$store.commit('isLoading', false);
    //}
  },
  methods: {
    test: function() {},
    deposit: function() {
      this.$store.commit("isLoading", true);
      this.$validator
        .validateAll({
          amount: this.amount
        })
        .then(result => {
          if (result === true) {
            this.axios
              .post(process.env.VUE_APP_API_Payment, {
                sessionId: this.$cookie.get("sessionId"),
                amount: this.amount
              })
              .then(response => {
                for (var i in response.data) {
                  this.successList = `Send ${this.amount} BTC to address ${
                    response.data[i]
                  }`;
                  break;
                }
                this.$store.commit("isLoading", false);
              })
              .catch(error => {
                for (var i in error.response.data["errors"]) {
                  this.errorList = error.response.data["errors"][i][0];
                  break;
                }
                this.$store.commit("isLoading", false);
              });
          }
        });

      this.$store.commit("isLoading", false);
    }
  },
  mounted() {
    //this.$store.commit('isLoading', true);
    //this.$socket.emit('requestingDepositMethod');
    //this.$store.commit('isLoading', false);
  }
};
</script>
