<template>
  <mdb-container fluid>
    <mdb-row class="primary-color" id="topC">
      <mdb-col col="4">
        <a v-bind:href="referrer" target="_blank"
          ><img src="Images/Logo.png" alt="Home" width="250" height="50"
        /></a>
      </mdb-col>
      <mdb-col class="text-center text-white font-weight-bold">
        <a v-bind:href="referrer" target="_blank" class="text-white"
          >{{ phrase }}
        </a>
      </mdb-col>
    </mdb-row>
    <mdb-row>
      <mdb-col>
        <iframe
          :src="this.link"
          width="100%"
          style="height: 100vh; border: none"
        ></iframe>
      </mdb-col>
    </mdb-row>

    <!--Squares-->

    <button
      type="button"
      class="bottomLeftButton"
      id="bottomLeftButton"
      onClick="document.getElementById('bottomLeftBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomLeftBanner" id="bottomLeftBanner">
      <div v-html="sbanner1"></div>
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
      <div v-html="sbanner2"></div>
    </div>

    <button
      type="button"
      class="bottomSecondRightButton"
      id="bottomSecondRightButton"
      onClick="document.getElementById('bottomSecondRightBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomSecondRightBanner" id="bottomSecondRightBanner">
      <div v-html="sbanner3"></div>
    </div>

    <button
      type="button"
      class="bottomSecondLeftButton"
      id="bottomSecondLeftButton"
      onClick="document.getElementById('bottomSecondLeftBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomSecondLeftBanner" id="bottomSecondLeftBanner">
      <div v-html="sbanner4"></div>
    </div>

    <!--Standards-->

    <button
      type="button"
      class="bottomStandardButton"
      id="bottomStandardButton"
      onClick="document.getElementById('bottomStandardBanner').remove();document.getElementById('bottomSecondStandardBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomStandardBanner" id="bottomStandardBanner">
      <div v-html="banner1"></div>
    </div>
    <div class="bottomSecondStandardBanner" id="bottomSecondStandardBanner">
      <div v-html="banner2"></div>
    </div>
  </mdb-container>
</template>
<script>
import { mdbCol, mdbRow, mdbContainer } from "mdbvue";
export default {
  components: { mdbCol, mdbRow, mdbContainer },
  data() {
    return {
      link: null,
      referrer: null,
      phrase:
        "Join today the best faucet site and claim massive BTC + the best pay to promote program in the same site as well",
      //phrase: 'Please do not use PTP to earn for now as there are some errors in counting valid hits for memebers and we will solve that error as soon as possible',
      //phrase2: 'If you are advertiser you can still advertise with our ptp system as we already have too many fixed ad spots in many sites that advertise our ptp rotator',
      banner1: null,
      banner2: null,
      sbanner1: null,
      sbanner2: null,
      sbanner3: null,
      sbanner4: null
    };
  },
  methods: {
    test: function() {},
    claimCredit: function() {
      grecaptcha
        .execute(process.env.VUE_APP_recaptchav3SiteKey, { action: "PTP" })
        .then(token => {
          this.axios
            .get(process.env.VUE_APP_Recaptchav3, {
              params: { recaptchav3Response: token }
            })
            .then(response => {
              let promise = new Promise(resolve => {
                setTimeout(
                  () =>
                    resolve(
                      `User ${
                        this.$store.state.Username
                      } has been credited for your visit here`
                    ),
                  6000
                );
              });
              promise.then(result => {
                this.$socket.emit(
                  "claimingPTPCredit",
                  JSON.stringify({
                    username: this.$store.state.Username,
                    ip: this.$store.state.ip,
                    valid: this.$store.state.Valid
                  })
                );
                this.phrase = result;
              });
            })
            .catch(error => {});
        });
    }
  },
  sockets: {
    receivingPTPLink: function(data) {
      this.link = data[0].Link;
      if (this.$store.state.Username != null) {
        this.claimCredit();
      }
    },
    receivingPTPType: function(data) {
      this.$store.commit("Valid", data[0].Valid);
      if (data[0].Link === "Invisible" && data[0].Valid === "true") {
        this.$router.push("Invisible");
      } else {
        this.$socket.emit(
          "requestingPTPLink",
          JSON.stringify({
            absoluteReferrer: this.$store.state.AbsoluteReferrer,
            referrer: this.$store.state.Referrer,
            username: this.$store.state.Username
          })
        );
      }
    }
  },
  mounted() {
    this.axios
      .get(process.env.VUE_APP_ipAPI)
      .then(response => {
        this.$store.commit("IP", response.data.ip);
      })
      .catch(error => {});
    console.log(document.referrer);
    this.$store.commit("AbsoluteReferrer", document.referrer);
    this.$store.commit("Referrer", document.referrer);
    this.$store.commit("Username", this.$route.query.referrer);

    this.referrer =
      "http://localhost:8081/#/?referrer=" + this.$store.state.Username;
    this.$socket.emit(
      "requestingPTPType",
      JSON.stringify({ referrer: this.$store.state.Referrer })
    );

    this.axios
      .get(process.env.VUE_APP_PTPBannerNetworkHTMLCodeApi)
      .then(response => {
        this.banner1 = response.data.HTMLCode;
      })
      .catch(error => {});
    this.axios
      .get(process.env.VUE_APP_PTPBannerNetworkHTMLCodeApi)
      .then(response => {
        this.banner2 = response.data.HTMLCode;
      })
      .catch(error => {});

    this.axios
      .get(process.env.VUE_APP_PTPSquareBannerNetworkHTMLCodeApi)
      .then(response => {
        this.sbanner1 = response.data.HTMLCode;
      })
      .catch(error => {});
    this.axios
      .get(process.env.VUE_APP_PTPSquareBannerNetworkHTMLCodeApi)
      .then(response => {
        this.sbanner2 = response.data.HTMLCode;
      })
      .catch(error => {});
    this.axios
      .get(process.env.VUE_APP_PTPSquareBannerNetworkHTMLCodeApi)
      .then(response => {
        this.sbanner3 = response.data.HTMLCode;
      })
      .catch(error => {});
    this.axios
      .get(process.env.VUE_APP_PTPSquareBannerNetworkHTMLCodeApi)
      .then(response => {
        this.sbanner4 = response.data.HTMLCode;
      })
      .catch(error => {});
  }
};
</script>
