<template>
  <div>
    <mdb-row>
      <mdb-col class="text-center d-flex flex-wrap">
        <mdb-row class="mx-auto">
          <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
          <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
        </mdb-row>

        <p class="font-weight-bold">
          Note: Claim 1 advertisement at a time, do not claim another ad until
          you have claimed the previous one and you see you have been credited
          the amount. <br />
        </p>

        <mdb-card
          v-for="bonusAd in this.$store.state.bonusAds"
          v-bind:key="bonusAd.Id"
          class="mx-auto m-5"
          border="primary"
          style="width: 250px; height:175px;"
        >
          <mdb-card-header color="primary-color" class="text-center">{{
            bonusAd.Title
          }}</mdb-card-header>
          <mdb-card-body class="text-center">
            <mdb-card-text>
              {{ bonusAd.Description }}
              <br />
              <mdb-btn
                v-on:click="claimBonusAd()"
                v-bind:id="bonusAd.Id"
                color="primary"
                size="sm"
                >Claim</mdb-btn
              >
            </mdb-card-text>
          </mdb-card-body>
        </mdb-card>
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
  mdbIcon,
  mdbInput,
  mdbCard,
  mdbCardHeader,
  mdbCardBody,
  mdbCardTitle,
  mdbCardText
} from "mdbvue";
export default {
  components: {
    mdbRow,
    mdbCol,
    mdbBtn,
    mdbIcon,
    mdbInput,
    mdbCard,
    mdbCardHeader,
    mdbCardBody,
    mdbCardTitle,
    mdbCardText
  },
  data() {
    return {
      recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
      recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
      apiResponse: null,
      errorList: null,
      successList: null,
      ip: "0.0.0.0"
    };
  },
  sockets: {
    receivingBonusAd: function(data) {
      this.$store.commit("isLoading", true);

      window.open(data[0].ResultantLink, "_blank");

      this.$store.commit("isLoading", false);
    }
  },
  methods: {
    test: function() {},
    claimBonusAd: function() {
      this.$store.commit("isLoading", true);

      this.$socket.emit(
        "claimingBonusAd",
        JSON.stringify({
          sessionId: this.$cookie.get("sessionId"),
          ip: this.ip,
          id: event.target.id
        })
      );

      this.$store.commit("isLoading", false);
    }
  },
  mounted() {
    this.axios
      .get(process.env.VUE_APP_API_IP)
      .then(response => {
        this.ip = response.data.ip;
      })
      .catch(error => {});
  }
};
</script>
