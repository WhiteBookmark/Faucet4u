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
          <mdb-col col="3"></mdb-col>
          <mdb-col class="text-center">
            <mdb-card border="secondary" class="mb-3">
              <mdb-card-header class="text-secondary">
                <span v-if="status">Status: Closed</span>
                <span v-else>Status: Open</span>
              </mdb-card-header>
              <mdb-card-body class="text-secondary">
                <mdb-card-title tag="h5">Subject: {{ subject }}</mdb-card-title>
                <mdb-card-text>
                  {{ message }}
                </mdb-card-text>
              </mdb-card-body>
              <mdb-card-footer
                border="secondary"
                tag="h5"
                class="text-secondary"
                >Created: {{ this.moment(dateTime) }}</mdb-card-footer
              >
            </mdb-card>

            <div v-for="value in replyList">
              <div v-if="value.Username === 'Admin'">
                <mdb-card border="primary" class="mb-3">
                  <mdb-card-header class="text-primary">Admin</mdb-card-header>
                  <mdb-card-body class="text-primary">
                    <mdb-card-text>
                      {{ value.Reply }}
                    </mdb-card-text>
                  </mdb-card-body>
                  <mdb-card-footer
                    border="primary"
                    tag="h5"
                    class="text-primary"
                    >{{ this.moment(value.DateTime) }}</mdb-card-footer
                  >
                </mdb-card>
              </div>
              <div v-else>
                <mdb-card border="secondary" class="mb-3">
                  <mdb-card-header class="text-secondary">{{
                    value.Username
                  }}</mdb-card-header>
                  <mdb-card-body class="text-secondary">
                    <mdb-card-text>
                      {{ value.Reply }}
                    </mdb-card-text>
                  </mdb-card-body>
                  <mdb-card-footer
                    border="secondary"
                    tag="h5"
                    class="text-secondary"
                    >{{ this.moment(value.DateTime) }}</mdb-card-footer
                  >
                </mdb-card>
              </div>
            </div>

            <mdb-card border="secondary" class="mb-3">
              <mdb-card-header class="text-secondary"
                >Reply form</mdb-card-header
              >
              <mdb-card-body class="text-secondary">
                <mdb-card-text>
                  <p class="font-weight-bold">Message</p>
                  <textarea
                    v-validate="'required|min:10'"
                    name="reply"
                    rows="6"
                    v-bind:disabled="status"
                    v-model="reply"
                    type="text"
                    class="form-control"
                  />

                  <span class="font-weight-bold">
                    {{ errors.first("reply") }}
                  </span>

                  <div class="text-center mt-4">
                    <mdb-btn
                      v-on:click="replyingSupportTicket()"
                      v-bind:disabled="status"
                      color="secondary"
                      >Reply</mdb-btn
                    >
                  </div>
                </mdb-card-text>
              </mdb-card-body>
            </mdb-card>
          </mdb-col>
          <mdb-col col="3"></mdb-col>
        </mdb-row>
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
import {
  mdbRow,
  mdbCol,
  mdbBtn,
  mdbCard,
  mdbCardHeader,
  mdbCardBody,
  mdbCardTitle,
  mdbCardText,
  mdbCardFooter
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
    mdbCardFooter
  },
  data() {
    return {
      recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
      recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
      subject: null,
      message: null,
      status: null,
      apiResponse: null,
      reply: null,
      dateTime: null,
      errorList: null,
      successList: null,
      replyList: []
    };
  },
  sockets: {
    receivingSupportTicket: function(data) {
      this.subject = data[0].Subject;
      this.message = data[0].Message;
      this.status = data[0].Locked;
      this.dateTime = data[0].DateTime;
    },
    receivingSupportTicketReplies: function(data) {
      this.replyList.length = 0;
      for (var i = 0; i < Object.keys(data).length; i++) {
        this.replyList.push({
          Username: data[i].Username,
          "Date Time": data[i].DateTime,
          Reply: data[i].Reply
        });
      }

      this.$store.commit("isLoading", false);
    },
    repliedSupportTicket: function(data) {
      this.$socket.emit(
        "requestingSupportTicketReplies",
        JSON.stringify({ reference: this.$store.state.viewTicketsReference })
      );
    }
  },
  created() {},
  methods: {
    test: function() {},
    replyingSupportTicket: function() {
      this.$validator.validateAll().then(result => {
        this.$store.commit("isLoading", true);

        if (result === true) {
          this.$socket.emit(
            "replyingSupportTicket",
            JSON.stringify({
              sessionId: this.$cookie.get("sessionId"),
              reference: this.$store.state.viewTicketsReference,
              reply: this.reply
            })
          );
        }
      });
    }
  },
  mounted() {
    this.$store.commit("isLoading", true);
    this.$socket.emit(
      "requestingSupportTicket",
      JSON.stringify({
        sessionId: this.$cookie.get("sessionId"),
        reference: this.$store.state.viewTicketsReference
      })
    );
    this.$socket.emit(
      "requestingSupportTicketReplies",
      JSON.stringify({ reference: this.$store.state.viewTicketsReference })
    );

    this.$store.commit("isLoading", false);
  }
};
</script>
