<template>
  <div>
    <mdb-row>
      <mdb-col col="1">
        <skyscrapper-banner></skyscrapper-banner>
      </mdb-col>
      <mdb-col class="text-center">
        <mdb-row>
          <mdb-col>
            <standard-banner></standard-banner>
            <br />
          </mdb-col>
          <mdb-col>
            <standard-banner></standard-banner>
            <br />
          </mdb-col>
        </mdb-row>
        <br />

        <div class="card text-white bg-secondary w-75 mx-auto">
          <div class="card-body">
            <a v-bind:href="[referralUrl]" target="_blank"
              ><img
                src="Images/Banner468Animated.gif"
                alt="Faucet4all - Claim every 3 minutes !"
            /></a>
            <br />
            <br />
            <textarea
              v-model="banner468AnimatedHTML"
              class="w-100"
              mdb-rows="3"
            ></textarea>
          </div>
        </div>
        <br />
        <div class="card text-white bg-secondary w-75 mx-auto">
          <div class="card-body">
            <a v-bind:href="[referralUrl]" target="_blank"
              ><img
                src="Images/Banner468Static.png"
                alt="Faucet4all - Claim every 3 minutes !"
            /></a>
            <br />
            <br />
            <textarea
              v-model="banner468StaticHTML"
              class="w-100"
              mdb-rows="3"
            ></textarea>
          </div>
        </div>
        <br />
        <div class="card text-white bg-secondary w-75 mx-auto">
          <div class="card-body">
            <a v-bind:href="[referralUrl]" target="_blank"
              ><img
                src="Images/Banner125Static.png"
                alt="Faucet4all - Claim every 3 minutes !"
            /></a>
            <br />
            <br />
            <textarea
              v-model="banner125StaticHTML"
              class="w-100"
              mdb-rows="3"
            ></textarea>
          </div>
        </div>
      </mdb-col>
      <mdb-col col="1">
        <skyscrapper-banner></skyscrapper-banner>
      </mdb-col>
      <mdb-col col="1"></mdb-col>
    </mdb-row>

    <mdb-row>
      <mdb-col>
        <standard-banner></standard-banner>
        <br />
        <standard-banner></standard-banner>
        <br />
        <standard-banner></standard-banner>
        <br />
      </mdb-col>
      <mdb-col>
        <standard-banner></standard-banner>
        <br />
        <standard-banner></standard-banner>
        <br />
        <standard-banner></standard-banner>
        <br />
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
import { mdbRow, mdbCol, Btn } from "mdbvue";

export default {
  components: { mdbRow, mdbCol, Btn },
  data() {
    return {
      recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
      recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
      address: this.$store.state.userData[0].FaucetHubBitcoinAddress,
      apiResponse: null,
      referralUrl: `http://localhost:8081/#/?referrer=${
        this.$store.state.userData[0].Username
      }`,
      banner468AnimatedHTML: `<a href='http://localhost:8081/#/?referrer=${
        this.$store.state.userData[0].Username
      }' target="_blank"><img src='http://localhost:8081/Images/Banner468Animated.gif' alt="Faucet4all - Claim every 3 minutes !" /></a>`,
      banner468StaticHTML: `<a href='http://localhost:8081/#/?referrer=${
        this.$store.state.userData[0].Username
      }' target="_blank"><img src='http://localhost:8081/Images/Banner468Static.png' alt="Faucet4all - Claim every 3 minutes !" /></a>`,
      banner125StaticHTML: `<a href='http://localhost:8081/#/?referrer=${
        this.$store.state.userData[0].Username
      }' target="_blank"><img src='http://localhost:8081/Images/Banner125Static.png' alt="Faucet4all - Claim every 3 minutes !" /></a>`,
      errorList: null,
      successList: null
    };
  },
  methods: {
    test: function() {}
  },
  mounted() {}
};
</script>
