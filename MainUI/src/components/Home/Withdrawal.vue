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
            <div class="input-group mb-3">
              <div class="input-group-prepend">
                <label class="input-group-text" for="inputGroupSelect01"
                  >Withdrawal Type</label
                >
              </div>
              <select
                class="browser-default custom-select"
                id="inputGroupSelect01"
                v-model="withdrawalType"
              >
                <option value="Standard" selected>Standard</option>
                <option value="Offerwall">Offerwall</option>
              </select>
            </div>

            <mdb-input
              v-if="type !== 'FaucetHubBitcoin' && type !== 'Bitcoin'"
              basic
              class="mb-3"
              v-validate="
                `required|min_value:${
                  this.$store.getters.faucetSettingValue(
                    'MinimumExchangeWithdrawal'
                  ).Value
                }|max_value:${this.$store.state.userData[0].Balance}`
              "
              name="amount"
              v-model.trim="amount"
              type="number"
            >
              <span class="input-group-text" slot="prepend"
                >Amount in Bitcoin</span
              >
            </mdb-input>

            <mdb-input
              v-else-if="withdrawalType === 'Standard'"
              basic
              class="mb-3"
              v-validate="
                `required|min_value:${
                  this.$store.getters.faucetSettingValue('MinimumWithdrawal')
                    .Value
                }|max_value:${this.$store.state.userData[0].Balance}`
              "
              name="amount"
              v-model.trim="amount"
              type="number"
            >
              <span class="input-group-text" slot="prepend"
                >Amount in Bitcoin</span
              >
            </mdb-input>

            <mdb-input
              v-else
              basic
              class="mb-3"
              v-validate="
                `required|min_value:${
                  this.$store.getters.faucetSettingValue(
                    'MinimumOfferwallWithdrawal'
                  ).Value
                }|max_value:${this.$store.state.userData[0].OfferwallBalance}`
              "
              name="amount"
              v-model.trim="amount"
              type="number"
            >
              <span class="input-group-text" slot="prepend"
                >Amount in Bitcoin</span
              >
            </mdb-input>

            <div class="input-group mb-3">
              <div class="input-group-prepend">
                <label class="input-group-text" for="inputGroupSelect02"
                  >Withdrawal Wallet</label
                >
              </div>
              <select
                class="browser-default custom-select"
                id="inputGroupSelect02"
                v-model="type"
              >
                <option value="Advcash">Advcash</option>
                <option value="Augur">Augur</option>
                <option value="Bitcoin">Bitcoin</option>
                <option value="Bitcoincash">Bitcoincash</option>
                <option value="Dash">Dash</option>
                <option value="Dogecoin">Dogecoin</option>
                <option value="Ethereum">Ethereum</option>
                <option value="Ethereumclassic">Ethereum Classic</option>
                <option value="FaucetHubBitcoin" selected>Faucethub</option>
                <option value="Golem">Golem</option>
                <option value="Lisk">Lisk</option>
                <option value="Litecoin">Litecoin</option>
                <option value="Payeer">Payeer</option>
                <option value="Perfect Money">Perfect Money</option>
                <option value="Zcash">Zcash</option>
              </select>
            </div>

            <mdb-input
              basic
              class="mb-3"
              v-validate="`required`"
              name="address"
              v-model.trim="address"
              type="text"
            >
              <span class="input-group-text" slot="prepend"
                >Withdrawal Address</span
              >
            </mdb-input>

            <span class="text-danger">
              {{ errors.first("amount") }} <br />
              {{ errors.first("address") }} <br />
              {{ errors.first("type") }} <br />
              {{ errorList }} <br />
            </span>

            <mdb-btn v-on:click="withdraw()" color="primary" size="lg"
              >Withdraw</mdb-btn
            >
          </mdb-col>
        </mdb-row>
      </mdb-col>
      <mdb-col col="1">
        <skyscrapper-banner></skyscrapper-banner>
      </mdb-col>
      <mdb-col col="1"></mdb-col>
    </mdb-row>

    <mdb-row>
      <mdb-col>
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
      </mdb-col>
      <mdb-col>
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
      </mdb-col>
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
import {
  mdbRow,
  mdbCol,
  mdbBtn,
  mdbCard,
  mdbCardHeader,
  mdbCardBody,
  mdbCardTitle,
  mdbCardText,
  mdbInput
} from "mdbvue";

export default {
  components: {
    mdbRow,
    mdbCol,
    mdbBtn,
    mdbCard,
    mdbCardHeader,
    mdbCardBody,
    mdbCardTitle,
    mdbCardText,
    mdbInput
  },
  data() {
    return {
      recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
      recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
      address: this.$store.state.userData[0].FaucetHubBitcoinAddress,
      amount: (0.0).toFixed(8),
      type: "FaucetHubBitcoin",
      withdrawalType: "Standard",
      errorList: null,
      successList: null
    };
  },
  methods: {
    test: function() {},
    withdraw: function() {
      if (this.withdrawalType === "Standard") {
        this.requestWithdrawal();
      } else if (this.withdrawalType === "Offerwall") {
        this.requestOfferwallWithdrawal();
      }
    },
    requestWithdrawal: function() {
      this.$store.commit("isLoading", true);
      this.$validator
        .validateAll({
          amount: this.amount,
          type: this.type,
          address: this.address
        })
        .then(result => {
          if (result === true) {
            this.$socket.emit(
              "requestingWithdrawal",
              JSON.stringify({
                sessionId: this.$cookie.get("sessionId"),
                amount: this.amount,
                type: this.type,
                address: this.address
              })
            );
          }
        });

      this.$store.commit("isLoading", false);
    },
    requestOfferwallWithdrawal: function() {
      this.$store.commit("isLoading", true);
      this.$validator
        .validateAll({
          amount: this.amount,
          type: this.type,
          address: this.address
        })
        .then(result => {
          if (result === true) {
            this.$socket.emit(
              "requestingOfferwallWithdrawal",
              JSON.stringify({
                sessionId: this.$cookie.get("sessionId"),
                amount: this.amount,
                type: this.type,
                address: this.address
              })
            );
          }
        });

      this.$store.commit("isLoading", false);
    }
  },
  mounted() {}
};
</script>
